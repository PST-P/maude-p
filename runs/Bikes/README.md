# Processing of the `Bikes` example

1. Move to the `pst-p/maude/parser` folder.
   ~~~~
   cd ~/git/pst-p/maude/parser
   ~~~~
2. Run the compiler.
   ~~~~
   ./p2maude.sh BIKES ../../p-0.5.maude ../../Bikes/ ../runs/Bikes/bikes.maude
   ~~~~
3. You can then move to the folder of the generated files.
   ~~~~
   cd ~/git/pst-p/maude/runs/Bikes/
   ~~~~
4. Run Maude and load the generated file. 
   ~~~~
   maude bikes.maude 
   ~~~~
5. Execute the test case of choice. 
   ~~~~
   frew [1000] execute(t10U100B, init) .
   frew [1000] execute(t100U1000B, init) .
   ~~~~

Alternatively, 
4. Run Maude and load the `bikes-run.maude` file. 
   ~~~~
   maude bikes-run.maude 
   ~~~~

4. The SMC can then be run with
   ~~~~
   umaudemc scheck bikes-preds.maude 'execute(t10U100B, init)' formula.quatex -a 0.99
   umaudemc scheck bikes-preds.maude 'execute(tcMultipleClients, init)' formula.quatex -a 0.999
   ~~~~

