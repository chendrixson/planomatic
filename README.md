# Planomatic

Tool to help planning against Microsoft's internal Azure Dev Ops instance.  For folks outside Microsoft's COSINE division, this could be used as a sample for the following tech:
* Connecting to Azure Dev Ops through Microsoft.VisualStudio.Services
* Using the MSIX packaging project with VS2019
* The Material Design WPF toolkit from here: https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit

For more information and a MSIX install, check it out here!
http://aka.ms/planomatic

### Builds
[Master Build Pipeline](https://chendrixson.visualstudio.com/PlanoBuild/_build?definitionId=7&_a=summary&branchFilter=4)

Builds for PRs automatically generate an MSIX package and .appinstaller file.  That package will check for updates on launch, and can be installed with this command: \
ms-appinstaller:?source=https://planomaticstorage.z5.web.core.windows.net/plano_<branch_name>.appinstaller

