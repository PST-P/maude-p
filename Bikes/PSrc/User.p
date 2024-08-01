machine User {
  var parking: Parking;
  var deposit: DepositManager;
  var bike: Bike;

  start state Initial {
    entry (input: (d: DepositManager, p: Parking)) {
      parking = input.p;
      deposit = input.d;
      goto WithoutBike;
    }
  }

  state WithoutBike {
    entry {
      send parking, book, this; 
    }

    on getBike do (b: Bike) {
      bike = b;
      goto WithBikeMoving;
    }

    on noBikeAvailable do {
      send parking, book, this; 
    }
  }

  state WithBikeMoving {
    entry {
      // rates: stop 5, break 1, music 20, light 20...  Out(WithBikeMoving) is 46
      var chosen, current: int;
      chosen = choose(46);
      current = 5;                 // stop 5
      if (chosen < current) {
        send bike, stop;
        goto WithBikeHalted;
      } // else {
      current = current + 1;       // break 1
      if (chosen < current) {
        send bike, obreak, bike;
        send deposit, obreak, bike;
        goto WithoutBike;
      } // else { 
      current = current + 20;      // music 20
      if (chosen < current) {
        send bike, music;
        goto WithBikeMoving;       
      } // else {                  // light 20
      send bike, light;
      goto WithBikeMoving;       
      // } } } }  
    }
  }

  state WithBikeHalted {
    entry {
      // rates: start 5, park 1, break 1, music 20, gps 10, guideApp 10, naviApp 10, light 10
      var chosen, current: int;
      chosen = choose(67);
      current = 5;              // start 5
      if (chosen < current) {
        send bike, ostart;
        goto WithBikeMoving;
      } // else { 
      current = current + 1;    // park 1
      if (chosen < current) {
        send bike, park, bike;
        send parking, park, bike;
        goto WithoutBike;
      } // else { 
      current = current + 1;   // break 1
      if (chosen < current) {
        send bike, obreak, bike;
        send deposit, obreak, bike;
        goto WithoutBike;
      } // else { 
      current = current + 20;  // music 20
      if (chosen < current) {
        send bike, music;
        goto WithBikeHalted;   
      } // else { 
      current = current + 10;  // gps 10
      if (chosen < current) {
        send bike, gps;
        goto WithBikeHalted;   
      } // else { 
      current = current + 10;  // guideApp 10
      if (chosen < current) {
        send bike, guideApp;
        goto WithBikeHalted;   
      } // else { 
      current = current + 10;  // naviApp 10
      if (chosen < current) {
        send bike, naviApp;
        goto WithBikeHalted;   
      } // else {              // light 10
      send bike, light;
      goto WithBikeHalted;     
    } // } } } } } } }
  }
}
