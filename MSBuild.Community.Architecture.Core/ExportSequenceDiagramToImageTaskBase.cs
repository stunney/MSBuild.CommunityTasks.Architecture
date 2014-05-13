using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;

namespace MSBuild.Community.Architecture.Core
{
    public abstract class ExportSequenceDiagramToImageTaskBase : Task
    {
        [Required]
        public virtual string ModelingProjectPath { get; set; }

        [Required]
        public virtual string OutputPath { get; set; }

        //Doing this IDisposable crap because that is the ONLY common base interface for both 2012 and 2013 SDK versions of the classes.
        //They don't derive from ANYTHING else :(

        //Strong assumptions, I know.  But hey, I wrote it ;)

        protected abstract IDisposable LoadModelingProject(string modelingProjectPath);
        protected abstract Metafile GetEntireImageMetafileFromDiagramFile(string diagramFilename, IDisposable modelingProject);
        protected abstract IEnumerable<string> GetModelingProjectFilenames(IDisposable modelingProject);

        protected virtual void SaveMetafileToFile(System.Drawing.Imaging.Metafile image, string output)
        {
            using (Stream s = new FileStream(output, FileMode.Create))
            {
                Log.LogMessage("Saving image as jpg to {0}", output);
                image.Save(s, ImageFormat.Jpeg);
            }
        }

        public override bool Execute()
        {
            //Visual studio requires this in order to execute this task! http://www.microsoft.com/en-us/download/confirmation.aspx?id=40754

            bool retval = false;

            try
            {
                Log.LogMessage("Loading modeling project {0}", ModelingProjectPath);

                IDisposable proj = LoadModelingProject(ModelingProjectPath);

                foreach (string diagramFilename in GetModelingProjectFilenames(proj))
                {
                    Log.LogMessage("Loading diagram file {0}", diagramFilename);
                    string output = string.Concat(OutputPath, System.IO.Path.DirectorySeparatorChar, new FileInfo(diagramFilename).Name, @".jpg");
                    SaveMetafileToFile(GetEntireImageMetafileFromDiagramFile(diagramFilename, proj), output);
                }
                Log.LogMessage("Done");

                retval = true;
            }
            catch (Exception _ex)
            {
                Log.LogError(_ex.Message);
            }

            return retval;
        }
    }
}