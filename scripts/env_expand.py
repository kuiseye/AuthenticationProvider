from sys import argv
from dotenv import dotenv_values
from pathlib import Path
if len(argv) < 2:
    raise ValueError("Source is missing")

if len(argv) < 3:
    raise ValueError("Destination is missing")

values = dotenv_values(argv[1])

path = Path(argv[2])

path.parent.mkdir(parents=True, exist_ok=True)
outF = open(path, "w")

for key,value in values.items():
    outF.write(f"{key}={value}\n")
outF.flush()
outF.close()