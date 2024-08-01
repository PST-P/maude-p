machine DepositManager {
  var price, weight, load: map[string, float];
  var maxPrice, maxWeight, maxLoad: float;
  var installRates: map[string, int];
  var uninstallRates: map[string, int];
  var replaceRates: map[string, map[string, int]];
  // alternatively, map[Bike, (float, float, float)]
  var bikePrice, bikeLoad, bikeWeight: map[Bike, float]; 
  var parking: Parking;

  start state Active {
    entry (input: (p: Parking, mPrice: float, mWeight: float, mLoad: float)) {
      maxPrice = input.mPrice;
      maxWeight = input.mWeight;
      maxLoad = input.mLoad;
      price = priceInit();
      weight = weightInit();
      load = loadInit();
      parking = input.p;
      installRates = installRatesInit("Deposit");
      uninstallRates = uninstallRatesInit("Deposit"); 
      replaceRates = replaceRatesInit("Deposit");
    }
    
    on newBike do (input: (bike: Bike, price: float, weight: float, load: float)) {
      bikePrice[input.bike] = input.price;
      bikeWeight[input.bike] = input.weight;
      bikeLoad[input.bike] = input.load;
      send this, received, input.bike; 
    }

    on received do (bike: Bike) { 
      Handle(bike);
      send bike, deploy, bike;
      send parking, deploy, bike; 
    }
    
    on obreak do (bike: Bike) {
      var chosen, current: int;
      // assistance 10, dump 1
      current = 10;
      chosen = choose(11);
      if (chosen < current) {
        send bike, assistance;
        Handle(bike);
        send bike, deploy, bike;
        send parking, deploy, bike;
      } else {
        send bike, dump;
        bikePrice -= bike;
        bikeWeight -= bike;
        bikeLoad -= bike;
      }
    }
  }

  fun Handle(bike: Bike) {
    var piece, replacement: string;
    var outv, current, chosen: int;
    var done, actionDone: bool;

    done = false;
    outv = Out(bike);
    while (!done) {
      current = 8; // deploy rate 
      chosen = choose(outv);
      if (chosen < current) { 
        done = true; 
      } else {
        actionDone = false;
        foreach (piece in keys(installRates)) {
          if (!actionDone && ValidInstall(bike, piece)) { 
            current = current + installRates[piece];
            if (chosen <= current) {
              actionDone = true;
              send bike, install, (piece = piece, price = price[piece], weight = weight[piece], load = load[piece]);
            }
          }
        }
        if (!actionDone) {
          foreach (piece in keys(uninstallRates)) {
            if (!actionDone && ValidUninstall(bike, piece)) {
              current = current + uninstallRates[piece];
              if (chosen <= current) {
                actionDone = true;
                send bike, uninstall, (piece = piece, price = price[piece], weight = weight[piece], load = load[piece]);
              }
            }
          }
        }
        if (!actionDone) {
          foreach (piece in keys(replaceRates)) {
            foreach (replacement in keys(replaceRates[piece])) {
              if (!actionDone && ValidReplace(bike, piece, replacement)) {
                current = current + replaceRates[piece][replacement];
                if (chosen <= current) {
                  actionDone = true;
                  send bike, replace, (pieceOld = piece, priceOld = price[piece], weightOld = weight[piece], loadOld = load[piece], pieceNew = replacement, priceNew = price[replacement], weightNew = weight[piece], loadNew = load[piece]);
                }
              }
            }
          }
        }
      }
      outv = Out(bike);
    } 
  }

  // Calculates the out value for a given state.
  // In this case we are looking at the given state of the Bike machine.
  fun Out(b: Bike): int {
    var piece, replacement: string;
    var outv: int;
    
    outv = 8; // deploy
    foreach(piece in keys(installRates)) {
      if (ValidInstall(b, piece)) {
        outv = outv + installRates[piece];
      }
    }
    foreach(piece in keys(uninstallRates)) {
      if (ValidUninstall(b, piece)) {
        outv = outv + uninstallRates[piece];
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

  // Checks if the installation of the given piece satisfies the constraint
  fun ValidUninstall(b: Bike, piece: string): bool {
    return true;
  }
}
