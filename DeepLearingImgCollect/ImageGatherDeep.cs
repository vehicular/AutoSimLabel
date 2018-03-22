using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityScript.Lang;

public class ImageGatherDeep : MonoBehaviour
{
    // 4k = 3840 x 2160   1080p = 1920 x 1080
    //  private int captureWidth = Screen.width;
    //  private int captureHeight = Screen.height;
    private int captureWidth = 1015;
    private int captureHeight = 703;
    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;


    public bool optimizeForManyScreenshots = true;

    public enum Format { RAW, JPG, PNG, PPM, XML, TXT };
    public Format format = Format.JPG;
    public Format format_txt = Format.TXT;

    // folder to write output (defaults to data path)
    public string sign_folder;

    public string img_info_folder;

    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private int counter = 0; // image #

    // commands
    private bool captureScreenshot = false;
    private bool captureVideo = false;


    // public Vector3 target;
    public GameObject target;
    Camera cam;

    private Graphics graphics;

    // public GameObject test;
    string filename;


    ProjectionTargetObj proTarObj;

    void Start()
    {

        cam = this.GetComponent<Camera>();
        print("screen Width :" + Screen.width + " the screen Hight :" + Screen.height);

        proTarObj = target.GetComponent<ProjectionTargetObj>();

        if (sign_folder == null || sign_folder.Length == 0)
        {
            sign_folder = Application.dataPath;
            if (Application.isEditor)
            {
                // put screenshots in folder above asset path so unity doesn't index the files
                var stringPath = sign_folder + "/..";
                sign_folder = Path.GetFullPath(stringPath);
            }
            sign_folder += "/signShots";//+classifyFolderName;
            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(sign_folder);
        }

        if (img_info_folder == null || img_info_folder.Length == 0)
        {
            img_info_folder = Application.dataPath;
            if (Application.isEditor)
            {
                // put screenshots in folder above asset path so unity doesn't index the files
                var stringPath = img_info_folder + "/..";
                img_info_folder = Path.GetFullPath(stringPath);
            }
            img_info_folder += "/img_info_folder";//+classifyFolderName;
            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(img_info_folder);
        }

    }

    // create a unique filename using a one-up variable
    private string uniqueFilename(int width, int height)
    {
        if (sign_folder == null || sign_folder.Length == 0)
        {
            sign_folder = Application.dataPath;
            if (Application.isEditor)
            {
                // put screenshots in folder above asset path so unity doesn't index the files
                var stringPath = sign_folder + "/..";
                sign_folder = Path.GetFullPath(stringPath);
            }
            sign_folder += "/screenshots";//+classifyFolderName;

            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(sign_folder);

            // count number of files of specified format in folder
            string mask = string.Format("screen_{0}x{1}*.{2}", width, height, format.ToString().ToLower());
            counter = Directory.GetFiles(sign_folder, mask, SearchOption.TopDirectoryOnly).Length;//-----
        }

        string filename;
        filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", sign_folder, width, height, counter, format.ToString().ToLower()); ;
        ++counter;
        return filename;
    }

    public string uniqueFilenameTxt(int width, int height)
    {
        if (img_info_folder == null || img_info_folder.Length == 0)
        {
            img_info_folder = Application.dataPath;
            if (Application.isEditor)
            {
                // put screenshots in folder above asset path so unity doesn't index the files
                var stringPath = img_info_folder + "/..";
                img_info_folder = Path.GetFullPath(stringPath);
            }
            img_info_folder += "/img_info_folder";//+classifyFolderName;

            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(img_info_folder);

            // count number of files of specified format in folder
            string mask = string.Format("screen_{0}x{1}*.{2}", width, height, format_txt.ToString().ToLower());
            counter = Directory.GetFiles(img_info_folder, mask, SearchOption.TopDirectoryOnly).Length;//-----
        }

        string filename;
        filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", img_info_folder, width, height, counter - 1, format_txt.ToString().ToLower()); ;
        ++counter;
        return filename;
    }

    public void CaptureScreenshot()
    {
        captureScreenshot = true;
    }

    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.transform.position);
        captureWidth = cam.pixelWidth;
        captureHeight = cam.pixelHeight;

        Vector3 screenPos11 = cam.WorldToScreenPoint(ProjectionTargetObj.centerPosAfterProj);
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.transform.position = ProjectionTargetObj.centerPosAfterProj;
        g.transform.localScale = new Vector3(.1f, .1f, .1f);
        //Vector3 prjScreenPos = cam.WorldToScreenPoint(g.transform.position);

        //---test----------
        // Debug.Log("target is :" + screenPos.x + " pixels from the left ");
        // Debug.Log("target is :" + (Screen.width-screenPos.x) + " pixels from the right ");
        // Debug.Log("target is :" + screenPos.y + "top");
        // Debug.Log("target is :" +(Screen.height - screenPos.y));


        //target.transform.Translate(new Vector3(0, 0.1f, 0));

        captureScreenshot |= Input.GetKeyDown("k");
        captureVideo = Input.GetKey("v");

        if (captureScreenshot || captureVideo)
        {
            captureScreenshot = false;

            // hide optional game object if set
            if (hideGameObject != null)
                hideGameObject.SetActive(false);

            // create screenshot objects if needed
            if (renderTexture == null)
            {
                // creates off-screen render texture that can rendered  into
                rect = new Rect(0, 0, captureWidth, captureHeight);
                renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
            }

            /*
            // get main camera and manually render scene into rt
            Camera camera = this.GetComponent<Camera>(); // NOTE: add because there was no reference to camera in original script; must add this script to Camera
            camera.targetTexture = renderTexture;
            camera.Render();
            */
            cam.targetTexture = renderTexture;
            cam.Render();

            // read pixels will read from the currently active render texture so make our offscreen
            // render texture active and then read the pixels
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(rect, 0, 0);

            // reset active camera texture and render texture
            cam.targetTexture = null;
            RenderTexture.active = null;

            // get our unique filename
            string filename = uniqueFilename((int)rect.width, (int)rect.height);

            // pull in our file header/data bytes for the specified image format(has to be done from main thread)
            byte[] fileHeader = null;
            byte[] fileData = null;
            if (format == Format.RAW)
            {
                fileData = screenShot.GetRawTextureData();
            }
            else if (format == Format.PNG)
            {
                fileData = screenShot.EncodeToPNG();
            }
            else if (format == Format.JPG)
            {
                fileData = screenShot.EncodeToJPG();
            }
            else // ppm
            {
                // create a file header for ppm formatted file
                string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
                fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
                fileData = screenShot.GetRawTextureData();
            }

            // create new thread to save the image to file (only  operation that can be done in background)
            new System.Threading.Thread(() =>
            {
                // create file and write optional header with image bytes
                var f = System.IO.File.Create(filename);
                if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
                f.Write(fileData, 0, fileData.Length);
                f.Close();
                Debug.Log(string.Format("Wrote screenshot {0} of size{ 1} ", filename, fileData.Length));
            }).Start();
            //string content = GetSize(target) + Angel(cam.transform, target) + Distance(cam.transform, target);
            string img_filename = uniqueFilenameTxt((int)rect.width, (int)rect.height);
            imgInfoSYS sys = new imgInfoSYS();
            sys.img_name = target.name;
            sys.objCenterPiexInImg_x = screenPos.x;
            sys.objCenterPiexInImg_y = cam.pixelHeight - screenPos.y;



            // Mesh obj_mesh = target.GetComponent<MeshFilter>().mesh;
            // Debug.Log(obj_mesh.name);
            meshfil meshFil = target.GetComponent<meshfil>();


            sys.obj_size_x = meshFil.car_size_x;
            sys.obj_size_y = meshFil.car_size_y;
            sys.obj_size_z = meshFil.car_size_z;


         
          

            sys.distance_to_cam = Distance(cam.transform, target);
            sys.angle_to_img = Angel(cam.transform, target);

           // Array targetPos = new Array[2];
          
           sys.target_pos_x = target.transform.position.x;
           sys.target_pos_y = target.transform.position.y;
           sys.target_pos_z = target.transform.position.z;
            //sys.target_POS = targetPos;

          //  Array camPos = new Array[2];

            sys.cam_pos_x = cam.transform.position.x;
            sys.cam_pos_y = cam.transform.position.y;
            sys.cam_pos_Z = cam.transform.position.z;


            sys.objProjectionHeight = ProjectionTargetObj.objProjectHeight;
            sys.objProjectionWidth = ProjectionTargetObj.objProjectWidth;
            //sys.cam_Pos = camPos;

            sys.prgCenterPiexInImg_x = (int)screenPos11.x;
            sys.prgCenterPiexInImg_y = cam.pixelHeight-(int)screenPos11.y;

            XMLWR.CreateImgXML(img_filename, sys);

            //---Txt file: Create_File(img_filename, content);

            // unhide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(true);

            // cleanup if needed
            if (optimizeForManyScreenshots == false)
            {
                Destroy(renderTexture);
                renderTexture = null;
                screenShot = null;
            }
        }
    }


    /* create TXT file
    public FileStream  Create_File(string filename,string content)
    {
        FileStream file = new FileStream(filename + ".txt",FileMode.Create ,FileAccess.ReadWrite);
        byte[] bytedata;
        //char[] chardata;
        // chardata = content.ToCharArray();
        //bytedata = new byte[chardata.Length];
        bytedata = System.Text.Encoding.Default.GetBytes(content);
        file.Write(bytedata, 0, bytedata.Length);
        file.Flush();
        file.Close();
        return file;        
    }
*/
    public float Angel(Transform origin_g, GameObject target_g)
    {
        Vector3 dir = target_g.transform.position - origin_g.position;
        float dot = Vector3.Dot(origin_g.forward, dir.normalized);
        float dot1 = Vector3.Dot(origin_g.right, dir.normalized);
        return Mathf.Acos(Vector3.Dot(origin_g.forward.normalized, dir.normalized)) * Mathf.Rad2Deg;
      // float V = Vector3.Angle(origin_g.transform.position, target_g.transform.position);
    }


    public float Distance(Transform origin, GameObject target)
    {
        return Vector3.Distance(origin.position, target.transform.position);
    }
}


