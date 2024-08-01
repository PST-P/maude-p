machine FactoryManager {
  var price, weight, load: map[string, float];
  var initialPrice, initialWeight, initialLoad: float;
  var maxPrice, maxWeight, maxLoad: float;
  var installRates: map[string, int];
  var replaceRates: map[string, map[string, int]];
  // alternatively, map[Bike, (float, float, float)]
  var bikePrice, bikeLoad, bikeWeight: map[Bike, float]; 
  var deposit: DepositManager;

  start state Active {
    entry (input: (iPrice: float, iWeight: float, iLoad: float, mPrice: float, mWeight: float, mLoad: float, d: DepositManager)) {
      initialPrice = input.iPrice;
      initialWeight = input.iWeight;
      initialLoad = input.iLoad;
      maxPrice = input.mPrice;
      maxWeight = input.mWeight;
      maxLoad = input.mLoad;
      price = priceInit();
      weight = weightInit();
      load = loadInit();
      installRates = installRatesInit("Factory");
      replaceRates = replaceRatesInit("Factory");
      deposit = input.d;
    }
    
    on create do (numBikes: int) {
      var b: Bike;

      while (numBikes > 0) {
        b = new Bike((initialPrice = initialPrice, initialWeight = initialWeight, initialLoad = initialLoad));
        bikePrice[b] = initialPrice;
        bikeWeight[b] = initialWeight;
        bikeLoad[b] = initialLoad;
        Handle(bike);
        send b, sell;
        send deposit, newBike, (bike = b, price = bikePrice[b], weight = bikeWeight[b], load = bikeLoad[b]);
        numBikes = numBikes - 1;
      }
    }
  }

  fun Handle(b: Bike) {
    var piece, replacement: string;
    var outv, current, chosen: int;
    var done, actionDone: bool;

    done = false;
    outv = Out(b);
    while (!done) {
      current = 8; // sell rate 
      chosen = choose(outv);
      if (chosen < current) { 
        done = true; 
      } else {
        actionDone = false;
        foreach (piece in keys(installRates)) {
          if (!actionDone && ValidInstall(b, piece)) {
            current = current + installRates[piece];
            if (chosen <= current) {
              actionDone = true;
              bikePrice[b] = bikePrice[b] + price[piece];
              bikeWeight[b] = bikeWeight[b] + weight[piece];
              bikeLoad[b] = bikeLoad[b] + load[piece];
              send b, install, (piece = piece, price = price[piece], weight = weight[piece], load = load[piece]);
            }
          }
        }
        if (!actionDone) {
          foreach (piece in keys(replaceRates)) {
            foreach (replacement in keys(replaceRates[piece])) {
              if (!actionDone && ValidReplace(b, piece, replacement)) {
                current = current + replaceRates[piece][replacement];
                if (chosen <= current) {
                  actionDone = true;
                  bikePrice[b] = bikePrice[b] - price[piece] + price[replacement];
                  bikeWeight[b] = bikeWeight[b] - weight[piece] + weight[replacement];
                  bikeLoad[b] = bikeLoad[b] - load[piece] + load[replacement];
                  send b, replace, (pieceOld = piece, priceOld = price[piece], weightOld = weight[piece], loadOld = load[piece], pieceNew = replacement, priceNew = price[replacement], weightNew = weight[replacement], loadNew = load[replacement]);
                }
              }
            }
          }
        }
      }
      outv = Out(b);
    } 
  }

  // Calculates the out value for a given state.
  // In this case we are looking at the given state of the Bike machine.
  fun Out(b: Bike): int {
    var piece, replacement: string;
    var outv: int;
    
    outv = 8; // sell
    foreach(piece in keys(installRates)) {
      if (ValidInstall(b, piece)) {
        outv = outv + installRates[piece];
      }
    }
    foreach(piece in keys(replaceRates)) {
      foreach(replacement in keys(replaceRates[piece])) {
        if (ValidReplace(b, piece, replacement)) {
          outv = outv + replaceRates[piece][replacement];
        }
      }
    }
    return outv;
  }

  // Checks if the installation of the given piece satisfies the constraint
  fun ValidInstall(b: Bike, piece: string): bool {
    return bikePrice[b] + price[piece] <= maxPrice
        && bikeWeight[b] + weight[piece] <= maxWeight
        && bikeLoad[b] + load[piece] <= maxLoad;
  }

  // Checks if the replacement of the given pieces satisfies the constraint
  fun ValidReplace(b: Bike, piece: string, replacement: string): bool {
    return bikePrice[b] - price[piece] + price[replacement] <= maxPrice
        && bikeWeight[b] - weight[piece] + weight[replacement] <= maxWeight
        && bikeLoad[b] - load[piece] + load[replacement] <= maxLoad;
  }
}

