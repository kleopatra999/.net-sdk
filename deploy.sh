cd CloudBoost && nuget pack CloudBoost.nuspec -Version $VERSION -IncludeReferencedProjects -Prop Configuration=Release && nuget push *.nupkg $NUGET_API_KEY -verbosity detailed
