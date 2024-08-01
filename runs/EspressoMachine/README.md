# Processing of the `EspressoMachine` example

1. Move to the `pst-p/maude/parser` folder.
   ~~~~
   cd ~/git/pst-p/maude/parser
   ~~~~
2. Run the compiler.
   ~~~~
   ./p2maude.sh ESPRESSO-MACHINE ../../p-0.5.maude ../../Tutorial/3_EspressoMachine/ ../runs/EspressoMachine/espresso-machine.maude
   ~~~~
3. You can then move to the folder of the generated files.
   ~~~~
   cd ~/git/pst-p/maude/runs/EspressoMachine/
   ~~~~
4. Run Maude and load the generated file. 
   ~~~~
   maude espresso-machine.maude 
   ~~~~
5. Execute the test case of choice. 
   ~~~~
   frew [1000] execute(tcSaneUserUsingCoffeeMachine, init) .
   ~~~~

Alternatively, 
4. Run Maude and load the `espresso-machine-run.maude` file. 
   ~~~~
   maude espresso-machine-run.maude 
   ~~~~

4. The SMC can then be run with
   ~~~~
   umaudemc scheck espresso-machine-preds.maude 'execute(tcSaneUserUsingCoffeeMachine, init)' formula.quatex -a 0.99
   umaudemc scheck espresso-machine-preds.maude 'execute(tcCrazyUserUsingCoffeeMachine, init)' formula.quatex -a 0.1
   ~~~~
