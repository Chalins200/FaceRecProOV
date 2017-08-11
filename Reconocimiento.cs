using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class Reconocimiento : Form
    {
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, t, NumLabels;
        string name, names = null;
        int band = 0;
       

        public Reconocimiento()
        {
            
                InitializeComponent();
                //Load haarcascades for face detection
                face = new HaarCascade("haarcascade_frontalface_default.xml");
                //eye = new HaarCascade("haarcascade_eye.xml");

                try
                {
                    //Load of previus trainned faces and labels for each image
                    string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                    string[] Labels = Labelsinfo.Split('%');
                    NumLabels = Convert.ToInt16(Labels[0]);
                    ContTrain = NumLabels;
                    string LoadFaces;

                    for (int tf = 1; tf < NumLabels + 1; tf++)
                    {
                        LoadFaces = "face" + tf + ".jpg";
                        trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                        labels.Add(Labels[tf]);
                    }

                }
                catch (Exception ee)
                {
                    //MessageBox.Show(e.ToString());
                    MessageBox.Show("La base de datos esta vacía, Captura o agrega imagenes a la base de datos", "Llenar base de datos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                
        }
        private Menu form = null;

        private Menu FormInstance
        {
            get
            {
                if (form == null)
                {
                    form = new Menu();
                    form.Disposed += new EventHandler(form_Disposed);
                }

                return form;
            }
        }
        
        void form_Disposed(object sender, EventArgs e)
        {
            form = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Menu frm = this.FormInstance;
            frm.Show();

            // si el Formulario estaba abierto seguramente este en segundo plano
            // con esta linea hacemos que pase adelante
            frm.BringToFront();
            this.Hide();
        }

        private void Reconocimiento_Load(object sender, EventArgs e)
        {
            //Initialize the capture device
            grabber = new Capture();
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
            this.Hide();
        }

        private void imageBoxFrameGrabber_Click(object sender, EventArgs e)
        {

        }

        private void btnDeteccion_Click(object sender, EventArgs e)
        {
            
        }

        public void FrameGrabber(object sender, EventArgs e)
        {

            NamePersons.Add("");

            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);

                    name = recognizer.Recognize(result);

                    //Draw the label for each face detected and recognized
                    currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }

                NamePersons[t - 1] = name;
                NamePersons.Add("");


                //Set the number of faces detected on the scene
                //label3.Text = facesDetected[0].Length.ToString();

                /*
                //Set the region of interest on the faces

                gray.ROI = f.rect;
                MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                   eye,
                   1.1,
                   10,
                   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                   new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp ey in eyesDetected[0])
                {
                    Rectangle eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
                 */

            }
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + ", ";
            }

            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            //label4.Text = names;
            names = "";
            //Clear the list(vector) of names
            NamePersons.Clear();

        }

        public static implicit operator Reconocimiento(FrmPrincipal v)
        {
            throw new NotImplementedException();
        }
    }
}
