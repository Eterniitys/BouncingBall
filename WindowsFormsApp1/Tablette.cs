using System;
using System.Collections.Generic;
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

		#region
		private VideoCapture _capture = null;
		private bool _captureInProgress;
		private bool _useThisFrame = false;
		private bool _calibrated = false;

		int markersX = 10;
		int markersY = 10;
		int markersRealLength = 25;
		int markersSeparation = 30;


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

		Mat _cameraMatrix = new Mat();
		Mat _distCoeffs = new Mat();
		Mat rvecs = new Mat();
		Mat tvecs = new Mat();

		private Size _imageSize = Size.Empty;

		private DetectorParameters _detectorParameters;
		#endregion
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
				_capture = new VideoCapture();
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
				Point capture_center = new Point(_imageSize.Height / 2, _imageSize.Width / 2);

				CvInvoke.CvtColor(_frame, _frameCopy, ColorConversion.Bgr2Gray);

				using (VectorOfInt ids = new VectorOfInt())
				using (VectorOfVectorOfPointF corners = new VectorOfVectorOfPointF())
				using (VectorOfVectorOfPointF rejected = new VectorOfVectorOfPointF()) {

					ArucoInvoke.DetectMarkers(_frameCopy, ArucoDictionary, corners, ids, _detectorParameters, rejected);
					//
					int nb_detected = ids.Size;
					int mLength = 0;

					PointF[] corners_pos = new PointF[nb_detected];
					double[] angles = new double[nb_detected];
					for (int k = 0; k < nb_detected; k++) {
						Point vector = new Point();
						vector.X = (int)(corners[k][1].X - corners[k][0].X + corners[k][2].X - corners[k][3].X);
						vector.Y = (int)(corners[k][1].Y - corners[k][0].Y + corners[k][2].Y - corners[k][3].Y);
						corners_pos[k] = corners[k][1];
						angles[k] = Math.Atan2(vector.Y, vector.X) * 180 / Math.PI;

						Point p = new Point((int)(corners[k][1].X), (int)(corners[k][1].Y));
						CvInvoke.Line(_frameCopy, p, new Point(p.X + vector.X, p.Y + vector.Y), new MCvScalar(0, 255, 0));

						int tmp_mLength = getNormfrom(vector, vector) / 2;
						mLength = mLength < tmp_mLength ? tmp_mLength : mLength;
					}

					double ratio = mLength / (double)markersRealLength;
					Point estimatedPos = new Point();
					for (int i = 0; i < nb_detected; i++) {
						estimatedPos.X = (int)((capture_center.X - corners_pos[i].X) / ratio);
						estimatedPos.Y = (int)((capture_center.Y - corners_pos[i].Y) / ratio);
					}
					if (nb_detected > 0) {
						this.setPosition(estimatedPos);
						this.setAngle(-(float)angles[0]);
					}
					//
					if (ids.Size > 0) {
						ArucoInvoke.RefineDetectedMarkers(_frameCopy, ArucoBoard, corners, ids, rejected, null, null, 10, 3, true, null, _detectorParameters);

						ArucoInvoke.DrawDetectedMarkers(_frameCopy, corners, ids, new MCvScalar(0, 255, 0));
						//ArucoInvoke.DrawDetectedMarkers(_frameCopy, rejected, new VectorOfInt(), new MCvScalar(0, 0, 255));

					}
				}
				_frameCopy.CopyTo(diplayableframe);
			} else {
				message = "VideoCapture was not created";
			}
		}

		private int getNormfrom(Point p1, Point p2) {
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
			TabletPositionChanged?.Invoke(pos);
		}

		/// <summary>
		/// set position in the room along the x axis
		/// </summary>
		/// <param name="pos_x"></param>
		public void setPosX(int pos_x) {
			this.position.X = pos_x;
		}

		/// <summary>
		/// set position in the room along the y axis
		/// </summary>
		/// <param name="pos_y"></param>
		public void setPosY(int pos_y) {
			this.position.Y = pos_y;
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
