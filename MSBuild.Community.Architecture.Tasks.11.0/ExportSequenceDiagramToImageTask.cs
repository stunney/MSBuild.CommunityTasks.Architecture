using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MSBuild.Community.Architecture.Core;

using Microsoft.VisualStudio.ArchitectureTools.Extensibility;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Build.Framework;

namespace MSBuild.Community.Architecture.Tasks._11._0
{
    public class ExportSequenceDiagramToImageTask : ExportSequenceDiagramToImageTaskBase, ITask
    {
        protected override IDisposable LoadModelingProject(string modelingProjectPath)
        {
            IModelingProjectReader proj = ModelingProject.LoadReadOnly(modelingProjectPath);
            return proj;
        }

        protected override Metafile GetEntireImageMetafileFromDiagramFile(string diagramFilename, IDisposable modelingProject)
        {
            IDiagram diag = ((IModelingProjectReader)modelingProject).LoadDiagram(diagramFilename);
            Log.LogMessage("Diagram loaded into interface.");
            Diagram d = diag.GetObject<Diagram>();
            ICollection selectedShapes = new[] { diag.GetObject<PresentationElement>() };
            return d.CreateMetafile(selectedShapes);            
        }

        protected override IEnumerable<string> GetModelingProjectFilenames(IDisposable modelingProject)
        {
            return ((IModelingProjectReader)modelingProject).DiagramFileNames;
        }
    }
}