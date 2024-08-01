event install: (piece: string, price: float, weight: float, load: float);
event uninstall: (piece: string, price: float, weight: float, load: float);
event replace: (pieceOld: string, priceOld: float, weightOld: float, loadOld: float, pieceNew: string, priceNew: float, weightNew: float, loadNew: float);
event sell;
event deploy: Bike;
event newBike: (bike: Bike, price: float, weight: float, load: float);
event received: Bike;
event create: int;
event maintain;
event book: User;
event music;
event light;
event obreak: Bike;
event ostart;
event stop;
event gps;
event naviApp;
event guideApp;
event park: Bike;
event assistance;
event dump;
event bike: Bike;
event noBikeAvailable;

fun priceInit(): map[string,float] {
  var price: map[string,float];
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

fun weightInit(): map[string,float] { // TODO: not defined for all values
  var weight: map[string,float];
  weight["AllYear"] = 0.3;
  weight["Summer"] = 0.2;
  weight["Winter"] = 0.4;
  weight["Light"] = 0.1;
  weight["Dynamo"] = 0.1;
  weight["Battery"] = 3.0;
  weight["Engine"] = 10.0;
  weight["MapsApp"] = 0.0; // can a default value be given in P?
  weight["NaviApp"] = 0.0; // can a default value be given in P?
  weight["GuideApp"] = 0.0; // can a default value be given in P?
  weight["Music"] = 0.0; // can a default value be given in P?
  weight["GPS"] = 0.0; // can a default value be given in P?
  weight["Basket"] = 0.5;
  weight["Diamond"] = 5.0;
  weight["StepThru"] = 3.5;
  return weight;
}

fun loadInit(): map[string,float] {
  var load: map[string,float];
  load["AllYear"] = 0.0; // can a default value be given in P?
  load["Summer"] = 0.0; // can a default value be given in P?
  load["Winter"] = 0.0; // can a default value be given in P?
  load["Light"] = 0.0; // can a default value be given in P?
  load["Dynamo"] = 0.0; // can a default value be given in P?
  load["Battery"] = 0.0; // can a default value be given in P?
  load["Engine"] = 0.0; // can a default value be given in P?
  load["MapsApp"] = 25.0;
  load["NaviApp"] = 55.0;
  load["GuideApp"] = 30.0;
  load["Music"] = 5.0;
  load["GPS"] = 10.0;
  load["Basket"] = 0.0; // can a default value be given in P?
  load["Diamond"] = 0.0; // can a default value be given in P?
  load["StepThru"] = 0.0; // can a default value be given in P?
  return load;
}

fun installRatesInit(st: string): map[string,int] {
  // same rates for both states
  var installRates: map[string, int];
  installRates["GPS"] = 6;
  installRates["MapsApp"] = 10;
  installRates["NaviApp"] = 6;
  installRates["GuideApp"] = 3;
  installRates["Music"] = 20;
  installRates["Engine"] = 4;
  installRates["Battery"] = 4;
  installRates["Dynamo"] = 10;
  installRates["Light"] = 10;
  installRates["Basket"] = 8;
  return installRates;
}

fun uninstallRatesInit(st: string): map[string,int] {
  // only available at the deposit state
  var uninstallRates: map[string, int];
  uninstallRates["GPS"] = 6;
  uninstallRates["MapsApp"] = 10;
  uninstallRates["NaviApp"] = 6;
  uninstallRates["GuideApp"] = 3;
  uninstallRates["Music"] = 20;
  uninstallRates["Engine"] = 1;
  uninstallRates["Battery"] = 2;
  uninstallRates["Dynamo"] = 3;
  uninstallRates["Light"] = 10;
  uninstallRates["Basket"] = 8;
  return uninstallRates;
}

fun replaceRatesInit(st: string) : map[string,map[string,int]] {
  var replaceRates: map[string, map[string,int]];
  var m: map[string,int];
  // replaceRates["AllYear"]["Summer"] = 5;
  // replaceRates["AllYear"]["Winter"] = 5;
  m["Summer"] = 5;
  m["Winter"] = 5;
  replaceRates["AllYear"] = m;
  // replaceRates["Summer"]["AllYear"] = 10;
  // replaceRates["Summer"]["Winter"] = 5;
  m = default(map[string,int]); // resets the map
  m["AllYear"] = 10;
  m["Winter"] = 5;
  replaceRates["Summer"] = m;
  // replaceRates["Winter"]["Summer"] = 5;
  // replaceRates["Winter"]["AllYear"] = 10;
   m = default(map[string,int]); // resets the map
  m["Summer"] = 5;
  m["AllYear"] = 10;
  replaceRates["Winter"] = m;
  if (st == "Factory") {
    // replaceRates["Diamond"]["StepThru"] = 3;
    m = default(map[string,int]); // resets the map
    m["StepThru"] = 3;
    replaceRates["Diamond"] = m;
    // replaceRates["StepThru"]["Diamond"] = 3;
    m = default(map[string,int]); // resets the map
    m["Diamond"] = 3;
    replaceRates["StepThru"] = m;
  } else {
    // replaceRates["Battery"]["Dynamo"] = 1;
    m = default(map[string,int]); // resets the map
    m["Dynamo"] = 3;
    replaceRates["Battery"] = m;
  }
  return replaceRates;
}