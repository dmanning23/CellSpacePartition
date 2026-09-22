CellSpacePartition
===================

[![NuGet](https://img.shields.io/nuget/v/CellSpacePartition.svg)](https://www.nuget.org/packages/CellSpacePartition/)

A MonoGame library that divides a 2D game world into a grid of cells, so you can run fast proximity/neighbor queries instead of testing every entity against every other entity. Useful for speeding up collision detection, steering behaviors (flocking, avoidance), and "what's near me" style gameplay logic.

## Why

Testing every entity against every other entity for proximity is O(n²) and gets slow fast. `CellSpacePartition` buckets entities into a uniform grid so a query only needs to look at the handful of cells that overlap the search area.

## Installation

Install the [CellSpacePartition NuGet package](https://www.nuget.org/packages/CellSpacePartition/):

```
dotnet add package CellSpacePartition
```

Targets `net8.0` and depends on `MonoGame.Framework.DesktopGL`, `RectangleF`, `PrimitiveBuddy`, `Vector2Extensions`, `MatrixExtensions`, and `RandomExtensions.dmanning23`.

## Usage

Any entity you want to track must implement `IMovingEntity`, exposing its current and previous position:

```csharp
public class Enemy : IMovingEntity
{
    public Vector2 Position { get; set; }
    public Vector2 OldPosition { get; set; }
}
```

Create a partition by specifying an origin, the size of each square cell, and how many cells wide/tall the grid is:

```csharp
// origin at (0,0), 64px cells, a grid 20 cells wide by 15 cells tall
var partition = new CellSpacePartition<Enemy>(Vector2.Zero, 64, 20, 15);

// add entities
partition.Add(enemy);

// each frame, after updating enemy.Position (and keeping OldPosition around),
// tell the partition so it can move the entity between cells if needed
partition.Update(enemy);

// remove an entity
partition.Remove(enemy);

// clear everything out
partition.Clear();
```

### Querying

```csharp
// all entities within queryRadius of targetPos
List<Enemy> nearby = partition.CalculateNeighbors(targetPos, queryRadius);

// fastQuery skips the per-entity distance check and just returns everything
// in any cell that overlaps the query area (cheaper, less precise)
List<Enemy> approxNearby = partition.CalculateNeighbors(targetPos, queryRadius, fastQuery: true);

// closest single entity within range
Enemy closest = partition.NearestNeighbor(targetPos, queryRadius);
```

Entities that fall outside the bounds of the grid are tracked separately as "floaters" and are still included in proximity queries, so nothing gets lost if it wanders off the edge of the world.

### Debug rendering

If you're using [PrimitiveBuddy](https://www.nuget.org/packages/PrimitiveBuddy/) to draw primitives, you can visualize the grid and query overlaps:

```csharp
partition.RenderCells(primitive);
partition.RenderCellIntersections(primitive, targetPos, queryRadius, Color.Red);
```

## Building

Open `CellSpacePartition.sln` in Visual Studio, or build from the command line:

```
dotnet build
```

## License

[MIT](LICENSE)
