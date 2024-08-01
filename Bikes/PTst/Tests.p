machine Test10U100B {
    start state Init {
        entry {
            var deposit: DepositManager;
            var factory: FactoryManager;
            var user: seq[User];
            var parking: Parking;
            var n: int;

            parking = new Parking();
            deposit = new DepositManager((p = parking, mPrice = 350.0, mWeight = 11.0, mLoad = 75.0));
            factory = new FactoryManager((iPrice = 1000.0, iWeight = 10.0, iLoad = 0.0, mPrice = 1350.0, mWeight = 21.0, mLoad = 75.0, d = deposit));
            n = 0;
            while (n < 10) {
              user += (n, new User((d = deposit, p = parking)));
              n = n + 1;
            }
            send factory, create, 100;
        }
    }
}

machine Test100U1000B {
    start state Init {
        entry {
            var deposit: DepositManager;
            var factory: FactoryManager;
            var user: seq[User];
            var parking: Parking;
            var n: int;

            parking = new Parking();
            deposit = new DepositManager((p = parking, mPrice = 350.0, mWeight = 11.0, mLoad = 75.0));
            factory = new FactoryManager((iPrice = 1000.0, iWeight = 10.0, iLoad = 0.0, mPrice = 1350.0, mWeight = 21.0, mLoad = 75.0, d = deposit));
            n = 0;
            while (n < 100) {
              user += (n, new User((d = deposit, p = parking)));
              n = n + 1;
            }
            send factory, create, 1000;
        }
    }
}