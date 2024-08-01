machine Parking {
  var bikes: seq[Bike];
  var numBikes: int;

  start state Initial {
    entry {
      numBikes = 0;
    }

    on deploy do (b: Bike) {
      bikes += (numBikes, b);
      numBikes = numBikes + 1;
    }

    // Bikes_0_11.txt A book event may arrive with no bikes available
    on book do (u: User) {
      var b: Bike;
      var ok: bool;
      while (!ok) {
        if (numBikes > 0) {
          numBikes = numBikes - 1;
          b = bikes[numBikes];
          if (choose(11) > 0) {
            send b, book, u;
            ok = true;
          } else {
            send b, maintain;
          }
        } else {
          send u, noBikeAvailable;
          ok = true;
        }
      }
    }
        
    on park do (b: Bike) {
      bikes += (numBikes, b);
      numBikes = numBikes + 1;
    }
  }
}