# cs ai

Yet another ai library, this time using C#.

See other ai projects (todo: links)

# Getting started

- install dotnet core, then

```
    dotnet build
    dotnet test
```

games/ has some runnable console projects. Use `dotnet run`
in their *.console directories.

# todo - non deterministic actions

- start: no dead ends, no repeated states, no non
  deterministic actions
- **DONE** solve search with start conditions
- **DONE** solve search with non deterministic actions
- **DONE** solve search with repeated states
- clean erratic search problem
- look for other todos
- pass vacuum world e2e test
- meh? spent too long on this already
    - solve search with avoidable dead ends
    - solve search with unavoidable dead ends