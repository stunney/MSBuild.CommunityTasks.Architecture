using System;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;

namespace MSBuild.Community.Architecture.Tasks
{
    public class ArchitectureDiagramsToImagesTask : Task
    {
        [Required]
        public virtual string ModelingProjectPath { get; set; }

        [Required]
        public virtual string OutputPath { get; set; }

        public override bool Execute()
        {
            //http://blogs.msdn.com/b/oscar_calvo/archive/2010/05/24/how-to-display-modeling-diagrams-outside-visual-studio.aspx
            //http://msdn.microsoft.com/en-us/library/ff469815.aspx
            //http://stackoverflow.com/questions/3009163/a-generic-error-occurred-in-gdi

            try
            {
                Log.LogMessage("Loading modeling project {0}", ModelingProjectPath);

                IModelingProjectReader proj = ModelingProject.LoadReadOnly(ModelingProjectPath);

                foreach (string diagramFilename in proj.DiagramFileNames)
                {
                    Log.LogMessage("Loading diagram file {0}", diagramFilename);
                    string output = string.Concat(OutputPath, System.IO.Path.DirectorySeparatorChar, new FileInfo(diagramFilename).Name, @".jpg");

                    IDiagram diag = proj.LoadDiagram(diagramFilename);
                    Log.LogMessage("Diagram loaded into interface.");
                    Diagram d = diag.GetObject<Diagram>();

                    ICollection selectedShapes = new[] { diag.GetObject<PresentationElement>() };
                    Log.LogMessage("Generating Metafile from Shapes collection.");
                    System.Drawing.Imaging.Metafile image = d.CreateMetafile(selectedShapes);
                    using (Stream s = new FileStream(output, FileMode.Create))
                    {
                        Log.LogMessage("Saving image as jpg to {0}", output);
                        image.Save(s, ImageFormat.Jpeg);
                    }
                }
                Log.LogMessage("Done");

                return true;
            }
            catch (Exception _ex)
            {
                Log.LogError(_ex.Message);
                return false;
            }
        }
    }
}