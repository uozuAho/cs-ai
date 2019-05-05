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
- **DONE** pass vacuum world e2e test
- clean erratic search problem
- delete previous attempts
- make as much private as possible
- look for other todos
- meh? spent too long on this already
    - solve search with avoidable dead ends
    - solve search with unavoidable dead ends