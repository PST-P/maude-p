# ISSUES 

- The compiler script must be executed from the `maude/parser` folder. Input and output files can be located anywhere else. Note also that we assume that Maude is available in the path using just `maude`. It is also assume that Maude's [unified model checker](https://github.com/fadoss/umaudemc) is also available. 

- The `p2maude.sh` script assumes that Python is invoked as `phyton3`. 

- Some projects may have some specifications in a `Commmons` folder. As pointed out before, our compiler only reads files from the `PSrc`, the `PSpec`, and the `PTst` folders. P files loaded from other locations must be copied into these folders to be compiled. We've copied all the files in the `Commons` folder used in the `FailureDetector` and `SimplePaxos` examples to their `PSpec` folders so that they can be compiled. 

- Monitors / spec machines are not currently supported. 
  It is for the moment left in the to-do list. 
  Since we are offering alternative forms of verification, it is not a priority at the moment. 
  They could be supported as well, but our goal is not to replace the existing P tools, but to complement them. 
  The support of monitors pose a non-trivial, but interesting challenge: since monitors are declared as observers on certain events, these event must be consumed simultaneously by the receiver of the event and the corresponding observers. 
  In theory, there could be multiple observers on the same event, which means that the receiver and all observers must synchronize on the reception of the event. 
  We believe it can easily be implemented using equations. 
  Announces do not pose any challenge, since they are addressed only to monitors, and there is no synchronization contraint on them. 

- The `EspressoMachine` example in P's tutorial was modified. 
In this case, it is possibly a mistake in the original example. 
I wonder how P's compiler goes through. 
Line 176 of the `CoffeMakerControlPanel.p` file is
  ~~~~
  assert CoffeeMakerReady == Ready;
  ~~~~
  However, `CoffeeMakerReady` is a state, and `Ready` one of the values of the enumerated type `tCoffeeMakerState`.
  ~~~~
  enum tCoffeeMakerState {
    NotWarmedUp,
    Ready,
    NoBeansError,
    NoWaterError
  }
  ~~~~
  We suppose that without the second argument, the assert doesn't do anything. 
  Or perhaps it should stop the execution, we don't know. We have `halt` for that. 
  We are just commenting line 176 out. 

- The P syntax for asserts is 
  ~~~~
  assert expr (, expr)? ;
  ~~~~~
  but we cannot handle the option in Maude. 
  In Muade, we only support 
  ~~~~
  assert expr , expr ;
  ~~~~~
  The P-to-Maude compiler takes a line like Line 175 of the `CoffeMakerControlPanel.p` file 
  ~~~~
  assert cofferMakerState != NotWarmedUp;
  ~~~~
  and write it into
  ~~~~
  assert cofferMakerState != NotWarmedUp, "";
  ~~~~

- There is an ambiguity for terms of the form `send E, E', E'' ;` since we have declarations 
  ~~~~
  op send_,_; : Expr EventName -> Sentence .
  op send_,_; : Expr VarId -> Sentence .
  op send_,_,_; : Expr EventName Expr -> Sentence .
  op send_,_,_; : Expr VarId VarId -> Sentence .
  ~~~~
  and therefore, a term like `send ms [i], ev, payload ;` may be parse like `send ms [i], ev, payload ;`, like `send (ms [i], ev), payload ;` or like `send ms [i], (ev, payload) ;`. Note that the second and fourth declarations are needed to handle cases like this one inwhich `ev` and `payload` are terms of type `VarId`. To handle them correctly, we are modifying the Maud emodule generated by the compiler so that the ambiguous terms are wirtten in their prefix-equivalent form. E.g., ```send_`,_`,_;(ms[i], ev, payload)```.

  In the the `two-phase-commit.maude` file, the following lines were manually modified:
  - Line 190, from 
    ~~~~
    send participants[i], message, payload ;
    ~~~~
    to
    ~~~~
    send_`,_`,_;(participants[i], message, payload)
    ~~~~
  - Line 261, from 
    ~~~~
    send target, message, payload ;
    ~~~~
    to
    ~~~~
    send_`,_`,_;(target, message, payload)
    ~~~~
  - Line 285, from 
    ~~~~
    send ms[i], ev, payload ;
    ~~~~
    to
    ~~~~
    send_`,_`,_;(ms[i], ev, payload)
    ~~~~
  Similar cases occur in the `FailureDetector` case study.  

- External functions are not supported. The `TwoPhaseCommit` project uses a function `ChooseRandomTransaction` written in C++ to generate random values. 

- Underlines not removed in operator `dead_nodes`, which comes from a payload name in the `Client` machine of the `FailureDetector` case study. 
  We are changing it by hand to `dead-nodes` as expected. 

- The `FailureDetector` example uses several features that we do not support (yet). 
  In our syntax, functions take lists of expressions, but records could be given as well. 
  For example, in line 48 of the `FailureDetector.p` file there is 
  ~~~~
  UnReliableBroadCast(notRespondedNodes, ePing, (fd =  this, trial = attempts));
  ~~~~
  Also, in line 7 of the `Node.p` file there is 
  ~~~~
  UnReliableSend(req.fd, ePong, (node = this, trial = req.trial));
  ~~~~
  This is used to send any payload, using the `any` type:
  ~~~~
  fun UnReliableSend(target : machine, message : event, payload : any)
  fun UnReliableBroadCast(ms : set[machine], ev : event, payload : any)
  ~~~~

- The `SimplePaxos` project does not compile. It possible doesn't compile in the P compiler either. In the state `ProposerPhaseOne` of the `Proposer` machine there is a `on eAgree do (proposal: tAgree)` when the type `tAgree` has not been defined anywhere. 

- +=, -=, *=, /= not supported (for numbers, += is supported for maps, sets and seqs)