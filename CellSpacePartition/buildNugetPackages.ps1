rm *.nupkg
nuget pack .\CellSpacePartition.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget push -source https://www.nuget.org -NonInteractive *.nupkg