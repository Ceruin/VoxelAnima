namespace VoxelAnima
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using Newtonsoft.Json;

    public partial class Form1 : Form
    {
        private Animation _animation = new Animation();
        private FlowLayoutPanel _framePanel = new FlowLayoutPanel();

        public Form1()
        {
            InitializeComponent();

            // Add the frame panel to the form
            _framePanel.Dock = DockStyle.Fill;
            this.Controls.Add(_framePanel);

            // Add the menu strip to the form
            var menuStrip = new MenuStrip();
            menuStrip.Items.Add("Load", null, btnLoadAnimation_Click);
            menuStrip.Items.Add("Save", null, btnSaveAnimation_Click);
            menuStrip.Items.Add("Export", null, btnExportAnimation_Click);
            this.Controls.Add(menuStrip);

            // Add the first empty frame to the panel
            AddEmptyFrame();
        }

        private void AddEmptyFrame()
        {
            var frameButton = new Button();
            frameButton.Text = "Empty";
            frameButton.Click += (sender, e) => btnLoadFrame_Click(sender, e, frameButton);
            _framePanel.Controls.Add(frameButton);
        }

        private void btnLoadFrame_Click(object sender, EventArgs e, Button frameButton)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "obj files (*.obj)|*.obj";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _animation.AddFrame(openFileDialog.FileName);
                frameButton.Text = Path.GetFileName(openFileDialog.FileName);

                // Add a new empty frame
                AddEmptyFrame();
            }
        }

        private void btnSaveAnimation_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "json files (*.json)|*.json";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(_animation));
            }
        }

        private void btnLoadAnimation_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json files (*.json)|*.json";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _animation = JsonConvert.DeserializeObject<Animation>(File.ReadAllText(openFileDialog.FileName));

                // Update the frame panel to match the loaded animation
                _framePanel.Controls.Clear();
                foreach (string frame in _animation.Frames)
                {
                    var frameButton = new Button();
                    frameButton.Text = Path.GetFileName(frame);
                    frameButton.Click += (sender, e) => btnLoadFrame_Click(sender, e, frameButton);
                    _framePanel.Controls.Add(frameButton);
                }
                AddEmptyFrame();
            }
        }

        private void btnExportAnimation_Click(object sender, EventArgs e)
        {
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "fbx files (*.fbx)|*.fbx";

            //if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    var scene = new Assimp.Scene();
            //    scene.RootNode = new Assimp.Node("Root");

            //    for (int i = 0; i < _animation.Frames.Count; i++)
            //    {
            //        string frameFile = _animation.Frames[i];

            //        // Load the .obj file for this frame
            //        var importer = new Assimp.AssimpContext();
            //        var frameScene = importer.ImportFile(frameFile);

            //        // Create a node for this frame
            //        var node = new Assimp.Node(Path.GetFileName(frameFile));
            //        node.Transform = Assimp.Matrix4x4.Identity;

            //        // Create an animation channel for this frame
            //        var channel = new Assimp.NodeAnimationChannel(Path.GetFileName(frameFile));
            //        channel.PositionKeys.Add(new Assimp.VectorKey(i, node.Position));
            //        channel.RotationKeys.Add(new Assimp.QuaternionKey(i, node.Orientation));
            //        channel.ScalingKeys.Add(new Assimp.VectorKey(i, node.Scaling));

            //        // Add the node and channel to the scene
            //        scene.RootNode.Children.Add(node);
            //        if (scene.AnimationCount > 0)
            //        {
            //            scene.Animations[0].NodeAnimationChannels.Add(channel);
            //        }
            //        else
            //        {
            //            var animation = new Assimp.Animation();
            //            animation.NodeAnimationChannels.Add(channel);
            //            scene.Animations.Add(animation);
            //        }
            //    }

            //    // Export the scene to an .fbx file
            //    var exporter = new Assimp.AssimpContext();
            //    exporter.ExportFile(scene, saveFileDialog.FileName, Assimp.PostProcessSteps.ValidateDataStructure);
            //}
        }

    }

    public class Animation
    {
        public List<string> Frames { get; } = new List<string>();

        public void AddFrame(string frame)
        {
            Frames.Add(frame);
        }
    }

}