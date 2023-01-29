# C patches
## how to build
- install [eac](https://gitlab.com/henny022/eac)
  - requires python 3.8+
  - some kind of virtual environment recommended
```
pip install git+https://gitlab.com/henny022/eac.git
```
- set up [decomp](https://github.com/zeldaret/tmc/) repo
- use make to build a single patch:
```
make <patch>.event REPO=<path/to/tmc/decomp/repo>
```
- or to build all patches:
```
make all REPO=<path/to/tmc/decomp/repo>
```
