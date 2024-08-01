# Processing of the `ClientServer` example

1. Move to the `pst-p/maude/parser` folder.
   ~~~~
   cd ~/git/pst-p/maude/parser
   ~~~~
2. Run the compiler.
   ~~~~
   ./p2maude.sh CLIENT-SERVER ../../p-0.5.maude ../../Tutorial/1_ClientServer/ ../runs/ClientServer/client-server.maude
   ~~~~
3. You can then move to the folder of the generated files.
   ~~~~
   cd ~/git/pst-p/maude/runs/ClientServer/
   ~~~~
4. Run Maude and load the generated file. 
   ~~~~
   maude client-server.maude 
   ~~~~
5. Execute the test case of choice. 
   ~~~~
   frew [1000] execute(tcSingleClient, init) .
   frew [1000] execute(tcMultipleClients, init) .
   ~~~~

Alternatively, 
4. Run Maude and load the `client-server-run.maude` file. 
   ~~~~
   maude client-server-run.maude 
   ~~~~

4. The SMC can then be run with
   ~~~~
   umaudemc scheck client-server-preds.maude 'execute(tcSingleClient, init)' formula.quatex -a 0.1
   umaudemc scheck client-server-preds.maude 'execute(tcMultipleClients, init)' formula.quatex -a 0.999
   ~~~~
