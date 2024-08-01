/* prueba 
   mas



   fdsfdsdsfdfdrewrewewerer
*/

type tWithDrawReq = (source: Client, accountId: int, amount: int, rId:int);
type tWithDrawResp = (status: tWithDrawRespStatus, accountId: int, balance: int, rId: int);

enum tWithDrawRespStatus {
  WITHDRAW_SUCCESS,
  WITHDRAW_ERROR
}

event eWithDrawReq : tWithDrawReq;
event eWithDrawResp: tWithDrawResp;

machine Client {
  var server : BankServer;
  var accountId: int;
  var nextReqId : int;
  var numOfWithdrawOps: int;
  var currentBalance: int;

  start state Init {
    entry (input : (serv : BankServer, accountId : int, bal : int)) {
      server = input . serv;
      currentBalance = input . balance;
      accountId = input . accountId;
      nextReqId = accountId * 100 + 1;
      goto WithdrawMoney;
    }
  }

  state WithdrawMoney {
    entry {
      var index : int;
      if (currentBalance <= 10)
         goto NoMoneyToWithDraw;

      send server, eWithDrawReq, (source = this, accountId = accountId, amount = WithdrawAmount(), rId = nextReqId);
      nextReqId = nextReqId + 1;
    }
  }

  state Prueba {
    entry (x : float) {
       var z : int;
       var g : bool;
       var mi : float;
       break;
       continue;
       return "10";
       return X[4*6];
       return X.4;
       return X.Y;
       return choose(5);
       return choose();
       return 5 as int;
       return X as map[int, int];
       return default(float);
       return format("hola", 5, 6);
       return (4, 5, "10");
       return (x1 = 5, x2 = 10, x4 = "H");
       return new XXXX(4, 6, "XSSS");
       return new YYYY();
       return F(3 + 4, 4, "hola");
       return keys(XX);
       x = 10;
       x.f1 = 20;
       x.3 = 200;
       x[2] = 102;
       x.y.f2 = "super";
       assert x == 10, 200;
       assert x > 100;
       print x + 30 + 40;
       foreach(y in 200){
          x = 100;
          z = x + 200 * 8;
        }

        i = 0;
        while(i < 100){
          print i;
          i = i + 1;
        }

        if(j != 11){
          j = j + 2;
          print j;
        }
        else{
          print "hola";
        }

        if(s == "super"){
           x[2] = 100 * y;
        }

        x += (i, a);
        x += (a);
        x -= (100);
        new XX();
        new XXX(200);
        f();
        f(3, 4, 3, "hola");
        raise halt;
        raise esAlgo, pick;
        send reader, eRWLockGranted, sharedObj;
        send reader, eRWLockGranted;
        announce esAlgo, pick;
        goto WaitForWithdrawRequests;
        goto WaitForWithdrawRequests, 100;
        receive {
          case eReadQueryResp, eSuperOh: (x : int, y : bool) {
            currentBalance = resp.balance;
          }

          case eReadQueryOther: (accountId: int, balance: int) {
            currentBalance = resp.balance;
          }
        }
    }

    exit {
      countPrepareResponses = 0;
      t[10] = 4 + 5;
    }
  }

  hot state Algo {
    entry Func;
    defer Func1, Func2;
    ignore Func3;

    on eReadTransReq, eReadOther do (rTrans : tReadTransReq) {
      send choose(participants), eReadTransReq, rTrans;
    }

    on algoMas do Funcioncita;
    on algoMax, otroRead goto StateIsland;
    on algoMax, otroRead goto StateIsland with Funcioncita;
    on eReadTransReq, eReadOther goto StateNew with (rTrans : tReadTransReq) {
      send choose(participants), eReadTransReq, rTrans;
    }
  }
}

machine BankServer {

}

fun BroadcastToAllPart(message: event, payload: any) : int {
  var i: int;
  while (i < sizeof(participants)) {
    send participants[i], message, payload;
    i = i + 1;
  }
}

fun BroadcastToAllParticipants(message: event, payload: any){
  var i: int;
  while (i < sizeof(participants)) {
    send participants[i], message, payload;
    i = i + 1;
  }
}

fun BroadcastToAllPart(message: event, payload: any) : int ;

fun BroadcastToAllPart(message: event, payload: any) ;

test tcSingleClient [main=TestWithSingleClient]:
  (union Client, Bank, { TestWithSingleClient });

test tcSingleClientOther [main=TestWithSingleClient]:
  assert BankBalanceIsAlwaysCorrect, GuaranteedWithDrawProgress in
  (union Client, Bank, { TestWithSingleClient });

test testLB_1 [main = TestDriver]:
  {User, TestDriver, LBSharedObject};

test tcSingleClient3 [main=TestWithSingleClient]:
  union Client, Bank, { TestWithSingleClient };

module Client = { Client };
module Bank = { BankServer, Database };
module AbstractBank = { AbstractBankServer -> BankServer };

spec GuaranteedWithDrawProgress observes eWithDrawReq, eWithDrawResp {
  var pendingWDReqs: set[int];

  start state NopendingRequests {
    on eWithDrawReq goto PendingReqs with (req: tWithDrawReq) {
      pendingWDReqs += (req.rId);
    }
  }

  cold state NopendingRequests {
    on eWithDrawReq goto PendingReqs with (req: tWithDrawReq) {
      pendingWDReqs += (req.rId);
    }
  }
}

spec GuaranteedWithDrawProgress observes eWithDrawReq, eWithDrawResp {
  var pendingWDReqs: set[int];

  start state NopendingRequests {
    on eWithDrawReq goto PendingReqs with (req: tWithDrawReq) {
      pendingWDReqs += (req.rId);
    }
  }

  hot state PendingReqs {
    on eWithDrawResp do (resp: tWithDrawResp) {
      assert resp.rId in pendingWDReqs,
        format ("unexpected rId: {0} received, expected one of {1}", resp.rId, pendingWDReqs);
      pendingWDReqs -= (resp.rId);
      if(sizeof(pendingWDReqs) == 0){
        goto NopendingRequests;
      }
    }

    on eWithDrawReq goto PendingReqs with (req: tWithDrawReq){
      pendingWDReqs += (req.rId);
    }
  }
}

spec BankBalanceIsAlwaysCorrect observes eWithDrawReq,  eWithDrawResp, eSpec_BankBalanceIsAlwaysCorrect_Init {
  // keep track of the bank balance for each client: map from accountId to bank balance.
  var bankBalance: map[int, int];
  // keep track of the pending withdraw requests that have not been responded yet.
  // map from reqId -> withdraw request
  var pendingWithDraws: map[int, tWithDrawReq];

  start state Init {
    on eSpec_BankBalanceIsAlwaysCorrect_Init goto WaitForWithDrawReqAndResp with (balance: map[int, int]){
      bankBalance = balance;
    }
  }

  state WaitForWithDrawReqAndResp {
    on eWithDrawReq do (req: tWithDrawReq) {
      assert req.accountId in bankBalance,
        format ("Unknown accountId {0} in the withdraw request. Valid accountIds = {1}",
          req.accountId, keys(bankBalance));
      pendingWithDraws[req.rId] = req;
    }

    on eWithDrawResp do (resp: tWithDrawResp) {
      assert resp.accountId in bankBalance,
        format ("Unknown accountId {0} in the withdraw response!", resp.accountId);
      assert resp.rId in pendingWithDraws,
        format ("Unknown rId {0} in the withdraw response!", resp.rId);
      assert resp.balance >= 10,
        "Bank balance in all accounts must always be greater than or equal to 10!!";

      if(resp.status == WITHDRAW_SUCCESS)
      {
        assert resp.balance == bankBalance[resp.accountId] - pendingWithDraws[resp.rId].amount,
          format ("Bank balance for the account {0} is {1} and not the expected value {2}, Bank is lying!",
            resp.accountId, resp.balance, bankBalance[resp.accountId] - pendingWithDraws[resp.rId].amount);
        bankBalance[resp.accountId] = resp.balance;
      }
      else
      {
        assert bankBalance[resp.accountId] - pendingWithDraws[resp.rId].amount < 10,
          format ("Bank must accept the withdraw request for {0}, bank balance is {1}!",
            pendingWithDraws[resp.rId].amount, bankBalance[resp.accountId]);
         assert bankBalance[resp.accountId] == resp.balance,
          format ("Withdraw failed BUT the account balance changed! actual: {0}, bank said: {1}",
            bankBalance[resp.accountId], resp.balance);
      }
    }
  }
}