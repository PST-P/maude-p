# Processing of the `TwoPhaseCommit` example
# IT DOESN'T WORK, IT USES EXTERNAL FUNCTIONS.

1. Move to the `pst-p/maude/parser` folder.
   ~~~~
   cd ~/git/pst-p/maude/parser
   ~~~~
2. Run the compiler.
   ~~~~
   ./p2maude.sh TWO-PHASE-COMMIT ../../p-0.5.maude ../../Tutorial/2_TwoPhaseCommit/ ../runs/TwoPhaseCommit/two-phase-commit.maude
   ~~~~
3. You can then move to the folder of the generated files.
   ~~~~
   cd ~/git/pst-p/maude/runs/TwoPhaseCommit/
   ~~~~
4. Run Maude and load the generated file. 
   ~~~~
   maude two-phase-commit.maude 
   ~~~~
5. Execute the test case of choice. 
   ~~~~
   frew [1000] execute(tcSingleClientNoFailure, init) .
   frew [1000] execute(tcMultipleClientsNoFailure, init) .
   frew [1000] execute(tcMultipleClientsWithFailure, init) .
   ~~~~

Alternatively, 
4. Run Maude and load the `two-phase-commit-run.maude` file. 
   ~~~~
   maude two-phase-commit-run.maude 
   ~~~~

