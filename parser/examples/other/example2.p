fun priceAux(x : int): map[string,float] {
  var price: map[string,float];
  var pieces: seq[string];
  var piece: bool;
  price["AllYear"] = 100.0;
  price["Summer"] = 70.0;
  price["Winter"] = 80.0;
  price["Light"] = 15.0;
  price["Dynamo"] = 40.0;
  price["Battery"] = 150.0;
  price["Engine"] = 300.0;
  price["MapsApp"] = 10.0;
  price["NaviApp"] = 20.0;
  price["GuideApp"] = 10.0;
  price["Music"] = 10.0;
  price["GPS"] = 20.0;
  price["Basket"] = 8.0;
  price["Diamond"] = 100.0;
  price["StepThru"] = 90.0;
  
  receive {
          case eReadQueryResp, eSuperOh: (x : int, y : bool) {
            var piece, totalPrice : int ;
            totalPrice = x + y + piece;
          }

          case eReadQueryOther: (accountId: int, balance: int) {
            piece = accountId * balance;
          }
          
          case algunoMas: {
            var totalPrice : float;
            piece = totalPrice * 2;
            receive {
               case caso1, caso2: {
                  var piece : bool;
                  print piece;
               }
            }
        }
     }
  
  
  return price;
}

machine Bike {
  var pieces: set[string];
  var totalPrice: float;
  var light, music, apps: bool;

  start state Factory {
    entry (initialPrice: float) {
      var algo, otro, totalPrice: int;
      totalPrice = initialPrice;
      light = false;
      music = false;
      apps = false;
    }

    on install do (input: (piece: string, price: float)) {
      var totalPrice : int;
      var light, music : int;
      print piece;
      input.piece = 10;
      input.5 = 20;
      totalPrice = input . 10;
      pieces += (input.piece);
      totalPrice = light;
      totalPrice = totalPrice + input.price;
    }

    on replace do (input: (pieceOld: string, priceOld: float, pieceNew: string, priceNew: float)) {
      pieces -= input.pieceOld;
      pieces += (input.pieceNew);
      totalPrice = totalPrice - input.priceOld + input.priceNew;
    }

    on sell do {
      var music : bool;
      if(music)
        totalPrice = totalPrice + 1;
      goto Deposit;
    }
    
    on sellito, otrico goto Deposit with {
      var music : bool;
      var totalPrice : float;
      if(music)
        totalPrice = totalPrice + 1;
      goto Deposit;
    }
    
    on sellito2, otrico2 goto Deposit with (a : int, b : float) {
      var music : bool;
      var totalPrice : float;
      if(music)
        totalPrice = totalPrice + a + b;
      goto Deposit;
    }
    
    exit {
      var music : bool;
      if(music)
        totalPrice = totalPrice + 1;
      goto Deposit;
    }
  }

  state Deposit {
    on install do (input: (piece: string, price: float)) {
      totalPrice = totalPrice + input.price;
      pieces += (input.piece);
    }

    on uninstall do (input: (piece: string, price: float)) {
      totalPrice = totalPrice - input.price;
      pieces -= (input.piece);
    }

    on replace do (input: (pieceOld: string, priceOld: float, pieceNew: string, priceNew: float)) {
      var pieces : set[string];
      totalPrice = totalPrice + input.priceNew - input.priceOld;
      pieces -= (input.pieceOld);
      pieces += (input.pieceNew);
    }

    on deploy do {
      goto Parked;
    }
  }

  state Parked {
    on maintain do {
      goto Deposit;
    }

    on book do {
      goto Moving;
    }
  }

  state Moving {
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

  state Halted {
    on ostart do {
      goto Moving;
    }

    on light do {
      light = !light;
    }

    on music do {
      music = !music;
    }

    on apps do {
      apps = ! apps;
    }
 
    on park do {
      goto Parked;
    }

    on obreak do {
      goto Broken;
    }
  }

  state Broken {
    on assistance do {
      goto Deposit;
    }

    on dump do {
      goto Trash;
    }
  }

  state Trash {

  }
  
  fun Out(b: Bike): int {
      var piece, replacement: string;
      var out: int;
      
      print out;	
      
      receive {
          case eReadQueryResp, eSuperOh: (x : int, y : bool) {
            var piece : int ;
            totalPrice = x + y + piece;
          }

          case eReadQueryOther: (accountId: int, balance: int) {
            piece = accountId * balance;
          }
          
          case algunoMas: {
            var totalPrice : float;
            piece = totalPrice * 2;
            receive {
               case caso1, caso2: {
                  var piece : bool;
                  print piece;
               }
            }
        }
     }
  }
  
  fun Out2(b: Bike): int {
      var piece, replacement: string;
      var out: int;
      
      print out + replacement;
      return b;
    }
    
  fun Out3(b: Bike): int {
      var piece, replacement, light: string;
      var out: int;
      
      print out + replacement + light;
      return b;
    }
    
  fun Out4(b: Bike): int {
      var piece, replacement: string;
      var out: int;
      
      print out + replacement + light;
      return b;
    }
  
}

fun priceInit(): map[string,float] {
  var price: map[string,float];
  var totalPrice: float;
  print totalPrice;
  price["AllYear"] = 100.0;
  price["Summer"] = 70.0;
  price["Winter"] = 80.0;
  price["Light"] = 15.0;
  price["Dynamo"] = 40.0;
  price["Battery"] = 150.0;
  price["Engine"] = 300.0;
  price["MapsApp"] = 10.0;
  price["NaviApp"] = 20.0;
  price["GuideApp"] = 10.0;
  price["Music"] = 10.0;
  price["GPS"] = 20.0;
  price["Basket"] = 8.0;
  price["Diamond"] = 100.0;
  price["StepThru"] = 90.0;
  return price;
}
