# TravelTime Internship Task - Location to Region Matcher

## Overview

This application matches locations to their corresponding regions based on geographic coordinates.  
Locations and regions are provided as JSON files. Each region can have multiple polygons, and each location can belong to multiple regions.  
The result is a JSON file listing all regions with their matched locations.

## Design Choices

- **Separation of Concerns:** The codebase is split into multiple files:
    - `Location.cs` and `Region.cs` for data models.
    - `Validator.cs` for input validation.
    - `PolygonUtils.cs` for geometric algorithms (ray casting).
    - `RegionMatcher.cs` for matching logic.
    - `Program.cs` for application entry point and I/O.
- **Streaming Deserialization:** Uses System.Text.Json's streaming API to efficiently process large input files without loading them entirely into memory.
- **Functional Patterns:** LINQ is used for concise, readable, and functional-style data processing.
- **Validation:** All input data is validated for structure, uniqueness, and correctness. Invalid data results in clear exceptions.
- **Extensibility:** The modular structure allows for easy extension, testing, and maintenance.

## Input & Output

### Input files

- **Locations** (`input/locations.json`):

    ```json
    [
      {
        "name": "<unique identifier>",
        "coordinates": [<longitude>, <latitude>]
      }
      // ... more locations
    ]
    ```

- **Regions** (`input/regions.json`):

    ```json
    [
      {
        "name": "<unique identifier>",
        "coordinates": [
          [[<longitude>, <latitude>], [<longitude>, <latitude>], ...]
          // ... more polygons
        ]
      }
      // ... more regions
    ]
    ```

### Output file

- **Results** (`output/results.json`):

    ```json
    [
      {
        "region": "<region identifier>",
        "matched_locations": [
          "<location identifier>",
          "<location identifier>"
        ]
      }
      // ... more regions
    ]
    ```

## How to Run

1. **Build the project:**
    ```sh
    dotnet build
    ```

2. **Run the matcher with input and output file paths:**
    ```sh
    dotnet run --project LocationRegionMatcher/LocationRegionMatcher.csproj LocationRegionMatcher/input/regions.json LocationRegionMatcher/input/locations.json LocationRegionMatcher/output/results.json
    ```

3. The output will be written to the specified results file.

## Notes

- Input files must be in the format described above.
- The application is robust against malformed input and large files.
- All edge cases (duplicate names, degenerate polygons, etc.) are handled and reported.

## Testing

Unit tests are provided in the `LocationRegionMatcher.Tests` project.  
To run all tests:

```sh
dotnet test
```

## Running in Docker

1. **Build the Docker image:**
    ```sh
    docker build -t location-region-matcher .
    ```

2. **Run the matcher:**
    ```sh
    docker run --rm -v "$PWD/LocationRegionMatcher/input:/app/input" -v "$PWD/LocationRegionMatcher/output:/app/output" location-region-matcher /app/input/regions.json /app/input/locations.json /app/output/results.json
    ```

- This mounts your local `input` and `output` folders into the container for file access.
- The output will be written to `LocationRegionMatcher/output/results.json` on your host.

---
