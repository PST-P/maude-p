# Processing of the `Bikes` example

1. Move to the `pst-p/maude/parser` folder.
   ~~~~
   cd ~/git/pst-p/maude/parser
   ~~~~
2. Run the compiler.
   ~~~~
   ./p2maude.sh DICE ../../p-0.5.maude ../../Dice/ ../runs/Dice/dice.maude
   ~~~~
3. You can then move to the folder of the generated files.
   ~~~~
   cd ~/git/pst-p/maude/runs/Dice/
   ~~~~
4. Run Maude and load the generated file. 
   ~~~~
   maude dice.maude 
   ~~~~
5. Execute the test case of choice. 
   ~~~~
   frew execute(tThrow, init) .
   frew execute(tNThrows, init) .
   frew execute(tThrow2, init) .
   frew execute(tNThrows2, init) .
   ~~~~

Alternatively, 
4. Run Maude and load the `dice-run.maude` file. 
   ~~~~
   maude dice-run.maude 
   ~~~~

4. The SMC can then be run with
   ~~~~
   umaudemc scheck dice-preds.maude 'execute(tNThrows2, init)' formula.quatex -a 0.999
   ~~~~

