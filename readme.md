# Piton

# Osnovne stvari
- [x] 2D, grid/mreža
    - [x] en tile je velik 32x32
- [x] kača se premika naprej
- [x] kača se obrne, če pritisnemo pravo tipko
    - [x] kača se ne more obrnit nazaj
    - [x] input batching
- [x] kača začne s tremi koščki, vsakič ko poje enega, se podaljša
- [x] ko se hrana poje, moremo ustvarit novo na random poziciji
- [ ] hrana se čez nekaj časa spremeni v kamen 
- [ ] če se kača zaleti v kamen, je game over
- [ ] če se kača "zaleti" v steno, se samo nadaljuje na drugi strani 
- [ ] če se kača zaleti v sebe, je opisano v drugem poglavju
- [ ] ko je game over, lahko začnemo znova

# Povečanje težavnosti, točke
- [ ] skozi igro nabiraš točke, ki so na koncu prikazane na end-screenu
- [ ] igra ima več "stopenj"
    - [ ] v vsaki stopnji je kača hitrejša
    - [ ] v vsaki stopnji dobi igralec več točk za vsako hrano
    - [ ] v novo stopnjo pridemo, ko je kača dovoj dolga in se odpre portal
    - [ ] kača spremeni barvo v vsaki stopnji, poleg točk piše stopnja in število točk za vsako hrano
- [ ] če se zaletiš
    - [ ] preostali rep se spremeni v hrano in izgubiš toliko točk, kot je vreden ta rep
    - [ ] rep se spremeni v hrano, ki jo lahko pobereš nazaj in dobiš nazaj vse točke
    - [ ] hrana se čez nekaj časa spremeni v kamen, ampak hitreje kot normalna hrana