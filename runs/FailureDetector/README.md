# Processing of the `FailureDetector` example
# We do not support sets of machine identifiers

1. Move to the `pst-p/maude/parser` folder.
   ~~~~
   cd ~/git/pst-p/maude/parser
   ~~~~
2. Run the compiler.
   ~~~~
   ./p2maude.sh FAILURE-DETECTOR ../../p-0.5.maude ~/git/p/Tutorial/4_FailureDetector/ ../runs/FailureDetector/failure-detector.maude
   ~~~~
   After running the compiler, `dead_nodes` must be replaced by `dead-nodes`, and two sends must be written in their equivalent prefix form. 
3. You can then move to the folder of the generated files.
   ~~~~
   cd ~/git/pst-p/maude/runs/FailureDetector/
   ~~~~
4. Run Maude and load the generated file. 
   ~~~~
   maude failure-detector.maude 
   ~~~~
5. Execute the test case of choice. 
   ~~~~
   frew [1000] execute(TestFailureDetector, init) .
   ~~~~

Alternatively, 
4. Run Maude and load the `failure-detector-run.maude` file. 
   ~~~~
   maude failure-detector-run.maude 
   ~~~~
