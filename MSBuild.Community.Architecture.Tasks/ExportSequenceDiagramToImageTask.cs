﻿using System;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;

using MSBuild.Community.Architecture.Core;

namespace MSBuild.Community.Architecture.Tasks._12._0
{
    public class ArchitectureDiagramsToImagesTask : ExportSequenceDiagramToImageTaskBase, ITask
    {
        //http://blogs.msdn.com/b/oscar_calvo/archive/2010/05/24/how-to-display-modeling-diagrams-outside-visual-studio.aspx
        //http://msdn.microsoft.com/en-us/library/ff469815.aspx
        //http://stackoverflow.com/questions/3009163/a-generic-error-occurred-in-gdi

        //Visual studio requires this in order to execute this task! http://www.microsoft.com/en-us/download/confirmation.aspx?id=40754

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