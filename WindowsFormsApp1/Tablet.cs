using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;

namespace BouncingBall {
	public class Tablet {

		public string message = "Nothing to say";

		#region "TODO"
		private VideoCapture _capture = null;

		int markersX = 10;
		int markersY = 10;
		int markersRealLength = 25;
		int markersSeparation = 30;

		VectorOfPoint markersRealPos = new VectorOfPoint(new Point[] {
			new Point(0,0),
			new Point(233,-4),
			new Point(468,-7),
			new Point(5,256),
			new Point(237,247),
			new Point(467,234),
			new Point(-6, 503),
			new Point(238,490),
			new Point(471,485),
		});


		private Dictionary _dict;
		private Dictionary ArucoDictionary {
			get {
				if (_dict == null)
					_dict = new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_100);
				return _dict;
			}
		}

		private GridBoard _gridBoard;
		private GridBoard ArucoBoard {
			get {
				if (_gridBoard == null) {
					_gridBoard = new GridBoard(markersX, markersY, markersRealLength, markersSeparation, ArucoDictionary);
				}
				return _gridBoard;
			}
		}

		Mat _frame = new Mat();
		Mat _frameCopy = new Mat();
		public Mat diplayableframe = new Mat();

		private Size _imageSize = Size.Empty;

		private DetectorParameters _detectorParameters;

		private readonly int idCamera = Properties.Settings.Default.iCameraId;
		private readonly int cannyThresholdLow = Properties.Settings.Default.iCannyThresholdLow;
		private readonly int cannyThresholdHight = Properties.Settings.Default.iCannyThresholdHight;
		public bool useHough { get; set; }

		#endregion

		#region Variables
		/// <summary>
		/// Call when the  angle event handler
		/// </summary>
		/// <param name="newAngle"></param>
		public delegate void TabletAngleChangedHandler(float newAngle);
		/// <summary>
		/// Thrown when the angle change
		/// </summary>
		public event TabletAngleChangedHandler TabletAngleChanged;
		/// <summary>
		/// The position event handler
		/// </summary>
		/// <param name="newPosition"></param>
		public delegate void TabletPositionChangedHandler(Point newPosition);
		/// <summary>
		/// Thrown when the position change
		/// </summary>
		public event TabletPositionChangedHandler TabletPositionChanged;

		/// <summary>
		/// tablet position in the room along the x axis and the y axis
		/// </summary>
		private Point position;
		/// <summary>
		/// The orientation of the tablet
		/// </summary>
		private float angle;
		/// <summary>
		/// 
		/// </summary>
		public ScreenFormat format { get; }

		public Ball ball;

		#endregion Variables
		/// <summary>
		/// Create a tablet simple representation 
		/// </summary>
		/// <param name="angle">The orientation on the tablet</param>
		/// <param name="format">Define the format use to represent this tablet in the room</param>
		public Tablet(int pos_x, int pos_y, float angle, ScreenFormat format, bool isMaster = false) {
			this.position = new Point(pos_x, pos_y);
			this.angle = angle;
			this.format = format;

			if (isMaster) {
				_detectorParameters = DetectorParameters.GetDefault();

				try {
					_capture = new VideoCapture(idCamera);
					if (!_capture.IsOpened) {
						_capture = null;
						throw new NullReferenceException("Unable to open video capture");
					} else {
						_capture.ImageGrabbed += processFrame;
						_capture.Start();
					}
				} catch (NullReferenceException excpt) {
					message = excpt.Message;
				}
			}
		}

		internal void moveBy(int delta_x, int delta_y) {
			this.position.X -= delta_x;
			this.position.Y -= delta_y;
		}

		private void processFrame(object sender, EventArgs arg) {
			if (_capture != null && _capture.Ptr != IntPtr.Zero) {
				_capture.Retrieve(_frame, 0);

				_imageSize = _frame.Size;
				Point capture_center = new Point(_imageSize.Width / 2, _imageSize.Height / 2);

				CvInvoke.CvtColor(_frame, _frameCopy, ColorConversion.Bgr2Gray);

				Mat cannyEdges = new Mat();
				Mat lines = new Mat();
				using (VectorOfInt ids = new VectorOfInt())
				using (VectorOfVectorOfPointF corners = new VectorOfVectorOfPointF())
				using (VectorOfVectorOfPointF rejected = new VectorOfVectorOfPointF()) {

					#region Markers detection / process position and angle
					ArucoInvoke.DetectMarkers(_frame, ArucoDictionary, corners, ids, _detectorParameters, rejected);
					//
					int nb_detected = ids.Size;
					int mLength = 0;

					PointF[] corners_pos = new PointF[nb_detected];
					double[] arucoAngles = new double[nb_detected];
					for (int k = 0; k < nb_detected; k++) {

						Point vector = new Point();
						vector.X = (int)(corners[k][1].X - corners[k][0].X + corners[k][2].X - corners[k][3].X);
						vector.Y = (int)(corners[k][1].Y - corners[k][0].Y + corners[k][2].Y - corners[k][3].Y);
						corners_pos[k] = corners[k][1]; // coin haut droit

						// TODO correction des angles par angle de pose réel du marker
						arucoAngles[k] = Math.Atan2(vector.Y, vector.X) * 180 / Math.PI;

						// Trace la ligne horizontale pour chaque marqueur utilisé pour le calcule de l'angle de la camera
						// - - - - - 
						Point p = new Point((int)(corners[k][1].X), (int)(corners[k][1].Y));
						CvInvoke.Line(_frame, p, new Point(p.X + vector.X, p.Y + vector.Y), new MCvScalar(0, 255, 0));
						// - - - - - 

						// détermine la longueur du coté de marqueur observé la plus longue
						// -> sert au calcule du ratio
						int tmp_mLength = getDist(vector, vector) / 2;
						mLength = mLength < tmp_mLength ? tmp_mLength : mLength;
						// <-
					}

					// cross at center image
					CvInvoke.Line(_frame,
						new Point(capture_center.X - 10, capture_center.Y),
						new Point(capture_center.X + 10, capture_center.Y),
						new MCvScalar(0, 0, 255), 2);
					CvInvoke.Line(_frame,
						new Point(capture_center.X, capture_center.Y - 10),
						new Point(capture_center.X, capture_center.Y + 10),
						new MCvScalar(0, 0, 255), 2);
					// - - - - - 

					double ratio = mLength / (double)markersRealLength;
					Point[] estimatedPosistion = new Point[nb_detected];

					float[] weights = new float[nb_detected];
					float sum = 0;

					PointF finalPosition = new PointF(0, 0);
					double finalAngle = 0f;
					for (int i = 0; i < nb_detected; i++) {
						#region position weighting
						estimatedPosistion[i].X = (int)((capture_center.X - corners_pos[i].X) / ratio + markersRealPos[ids[i]].X);
						estimatedPosistion[i].Y = (int)((capture_center.Y - corners_pos[i].Y) / ratio + markersRealPos[ids[i]].Y);
						weights[i] = 1f / getDist(estimatedPosistion[i], capture_center);
						finalPosition.X += estimatedPosistion[i].X * weights[i];
						finalPosition.Y += estimatedPosistion[i].Y * weights[i];
						#endregion position weighting
						#region angle weighting
						finalAngle += arucoAngles[i] * weights[i];
						#endregion angle weighting
						sum += weights[i];
					}
					finalPosition.X /= sum;
					finalPosition.Y /= sum;
					finalAngle /= sum;

					#endregion Markers detection / process position and angle

					CvInvoke.Canny(_frameCopy, cannyEdges, cannyThresholdLow, cannyThresholdHight);
					if (mLength != 0) {
						CvInvoke.HoughLines(cannyEdges, lines, 1, Math.PI / 180, (int)(2 * mLength));
						double[] houghAngles = new double[lines.Rows];
						for (int i = 0; i < lines.Rows; i++) {
							float theta = (float)lines.GetData().GetValue(new int[] { i, 0, 1 });
							houghAngles[i] = -theta / Math.PI * 180;
							// - - - - - - affichage de la ligne
							float rho = (float)lines.GetData().GetValue(new int[] { i, 0, 0 });
							float a = (float)Math.Cos(theta);
							float b = (float)Math.Sin(theta);

							PointF p = new PointF(a * rho, b * rho);

							Point pt1 = new Point((int)(p.X + 1000 * (-b)), (int)(p.Y + 1000 * a));
							Point pt2 = new Point((int)(p.X - 1000 * (-b)), (int)(p.Y - 1000 * a));

							//CvInvoke.Line(_frame, pt1, pt2, new MCvScalar(255, 0, 0), 1, LineType.AntiAlias);
							// - - - - - -
						}
						if (useHough)
							finalAngle = processArucoHoughAngles(finalAngle, houghAngles);
					}



					if (ids.Size > 0) {
						this.setPosition((int)finalPosition.X, (int)finalPosition.Y);
						// set the tablet properties
						this.setAngle(-(float)finalAngle);
						//
						ArucoInvoke.RefineDetectedMarkers(_frameCopy, ArucoBoard, corners, ids, rejected, null, null, 10, 3, true, null, _detectorParameters);

						ArucoInvoke.DrawDetectedMarkers(_frame, corners, ids, new MCvScalar(0, 255, 0));
					} else {
						// fix a weird error comming from 0(float) cast to 0(int) when markers aren't detected
						//this.setPosition(this.getPosition());
					}
				}
				_frame.CopyTo(diplayableframe);
			} else {
				message = "VideoCapture was not created";
			}
		}

		private double processArucoHoughAngles(double arucoAngle, double[] houghAngles) {
			if (houghAngles.Length == 0) {
				return arucoAngle;
			}
			double[] possibilities = new double[houghAngles.Length * 4];
			for (int i = 0; i < houghAngles.Length; i++) {
				//houghAngles[i] += 90;
				possibilities[i * 4] = houghAngles[i];
				possibilities[i * 4 + 1] = (houghAngles[i] + 90);
				possibilities[i * 4 + 2] = (houghAngles[i] + 180);
				possibilities[i * 4 + 3] = (houghAngles[i] + 270);
				message = string.Format("{0} | {1} | {2} | {3}", possibilities[0], possibilities[1], possibilities[2], possibilities[3]);
			}
			double dist = angleDist(possibilities[0], arucoAngle);
			double min_angle = arucoAngle;
			foreach (double angle in possibilities) {
				double d = angleDist(angle, arucoAngle);
				if (d < dist) {
					dist = d;
					min_angle = angle;
				}
			}
			return -min_angle;
		}

		private double angleDist(double angle1, double angle2) {
			double radAng1 = angle1 / 180 * Math.PI;
			double radAng2 = angle2 / 180 * Math.PI;
			double[] p1 = { Math.Cos(radAng1), Math.Sin(radAng1) };
			double[] p2 = { Math.Cos(radAng2), Math.Sin(radAng2) };
			return p1[0] * p2[0] + p1[1] * p2[1];
		}

		private int getDist(Point p1, Point p2) {
			return (int)Math.Sqrt(p1.X * p2.X + p1.Y * p2.Y);
		}

		#region Ascesseurs
		public Point getPosition() {
			return this.position;
		}

		/// <summary>
		/// Get position in the room along the x axis
		/// </summary>
		/// <returns></returns>
		public int getPosX() {
			return this.position.X;
		}

		/// <summary>
		/// Get position in the room along the y axis
		/// </summary>
		/// <returns></returns>
		public int getPosY() {
			return this.position.Y;
		}

		/// <summary>
		/// Get the orientation in the room
		/// </summary>
		/// <returns></returns>
		public float getAngle() {
			return this.angle;
		}

		/// <summary>
		/// Get the width of the sreen depending on the <see cref="format"/>
		/// </summary>
		/// <returns></returns>
		public int getWidth() {
			return Format.GetFormat(this.format)[0];
		}

		/// <summary>
		/// Get the height of the sreen depending on the <see cref="format"/>
		/// </summary>
		/// <returns></returns>
		public int getHeight() {
			return Format.GetFormat(this.format)[1];
		}

		private void setPosition(Point pos) {
			this.position = pos;
			TabletPositionChanged?.Invoke(this.position);
		}

		private void setPosition(int pos_x, int pos_y) {
			this.position.X = pos_x;
			this.position.Y = pos_y;
			TabletPositionChanged?.Invoke(this.position);
		}

		/// <summary>
		/// set position in the room along the x axis
		/// </summary>
		/// <param name="pos_x"></param>
		public void setPosX(int pos_x) {
			this.position.X = pos_x;
			TabletPositionChanged?.Invoke(this.position);
		}

		/// <summary>
		/// set position in the room along the y axis
		/// </summary>
		/// <param name="pos_y"></param>
		public void setPosY(int pos_y) {
			this.position.Y = pos_y;
			TabletPositionChanged?.Invoke(this.position);
		}

		/// <summary>
		/// Set the orientation in the room
		/// </summary>
		/// <param name="angle">In degree</param>
		public void setAngle(float angle) {
			this.angle = angle;
			TabletAngleChanged?.Invoke(angle);
		}


		#endregion
	}
}
