machine Bike {
  var pieces: set[string];
  var price, weight, load: float;
  var light, music, gps, guideApp, naviApp: bool;

  start state Init {
    entry (input: (initialPrice: float, initialWeight: float, initialLoad: float)) {
        goto Factory, input;
    }
  }

  hot state Factory {
    entry (input: (initialPrice: float, initialWeight: float, initialLoad: float)) {
      price = input.initialPrice;
      weight = input.initialWeight;
      load = input.initialLoad;
      light = false;
      music = false;
      gps = false;
      guideApp = false;
      naviApp = false;
    }

    on install do (input: (piece: string, price: float, weight: float, load: float)) {
      pieces += (input.piece);
      price = price + input.price;
      weight = weight + input.weight;
      load = load + input.load;
    }

    on replace do (input: (pieceOld: string, priceOld: float, weightOld: float, loadOld: float, pieceNew: string, priceNew: float, weightNew: float, loadNew: float)) {
      pieces -= input.pieceOld;
      pieces += (input.pieceNew);
      price = price - input.priceOld + input.priceNew;
      weight = weight - input.weightOld + input.weightNew;
      load = load - input.loadOld + input.loadNew;
    }

    on sell do {
      goto Deposit;
    }
  }

  hot state Deposit {
    on install do (input: (piece: string, price: float, weight: float, load: float)) {
      price = price + input.price;
      weight = weight + input.weight;
      load = load + input.load;
      pieces += (input.piece);
    }

    on uninstall do (input: (piece: string, price: float, weight: float, load: float)) {
      price = price - input.price;
      weight = weight - input.weight;
      load = load - input.load;
      pieces -= (input.piece);
    }

    on replace do (input: (pieceOld: string, priceOld: float, weightOld: float, loadOld: float, pieceNew: string, priceNew: float, weightNew: float, loadNew: float)) {
      price = price - input.priceOld + input.priceNew;
      weight = weight - input.weightOld + input.weightNew;
      load = load - input.loadOld + input.loadNew;
      pieces -= (input.pieceOld);
      pieces += (input.pieceNew);
    }

    on deploy do {
      goto Parked;
    }
  }

  hot state Parked {
    on maintain do {
      goto Deposit;
    }

    on book do (u: User) {
      send u, bike, this;
      goto Moving;
    }
  }

  hot state Moving {
    on light do {
      light = !light;
    }

    on music do {
      music = !music;
    }

    on stop do {
      goto Halted;
    }

    on obreak do {
      goto Broken;
    }
  }

  hot state Halted {
    on ostart do {
      goto Moving;
    }

    on light do {
      light = !light;
    }

    on music do {
      music = !music;
    }

    on gps do {
      gps = ! gps;
    }
 
    on guideApp do {
      guideApp = ! guideApp;
    }
 
    on naviApp do {
      naviApp = ! naviApp;
    }
 
    on park do {
      goto Parked;
    }

    on obreak do {
      goto Broken;
    }
  }

  hot state Broken {
    on assistance do {
      goto Deposit;
    }

    on dump do {
      goto Trash;
    }
  }

  state Trash {
    entry {
    }
  }
}
