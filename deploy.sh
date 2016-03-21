cd net-sdk && nuget pack net-sdk.nuspec -Version $VERSION -IncludeReferencedProjects -Prop Configuration=Release && nuget push *.nupkg $NUGET_API_KEY -verbosity detailed
