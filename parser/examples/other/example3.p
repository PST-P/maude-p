machine Bike {
  var pieces: set[string];
  var price, weight, load: float;
  var light, music, gps, guideApp, naviApp: bool;
  var maxPrice: float;
  var initialPrice: string;

  start state Init {
    entry (input: (initialPrice: bool, initialWeight: float, initialLoad: float)) {
        goto Factory, input;
        initialPrice = input.initialPrice;
    }
  }
  
  hot state Factory {
    entry (input: (initialPrice: float, initialWeight: float, initialLoad: float)) {
      var price : int;
      price = input.initialPrice;
      weight = input.initialWeight;
      load = input.initialLoad;
      light = false;
      music = false;
      gps = false;
      guideApp = false;
      naviApp = false;
    }
    
    on install, replace do (input: (piece: string, price: float, weight: float, load: float)) {
      pieces += (input.piece);
      price = price + input.price;
      weight = weight + input.weight;
      load = load + input.load;
    }
    
    on deploy do (b: Bike) {
      var bikes, numBikes : float;
      bikes += (numBikes, b);
      numBikes = numBikes + 1;
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

machine User {
  var bike: Bike;
}

event install: (piece: string, price: float, weight: float, load: float);
event uninstall: (piece: string, price: float, weight: float, load: float, pieceOld: int);
event replace: (pieceOld: string, priceOld: float, weightOld: float, loadOld: float, pieceNew: string, priceNew: float, weightNew: float, loadNew: float);
event deploy: Bike;
event sell;
event maintain;
event book: User;
event light;
event music;
event stop;
event obreak: Bike;
event ostart;
event gps;
event guideApp;
event naviApp;
event park: Bike;
event assistance;
event dump;
event bike: Bike;
