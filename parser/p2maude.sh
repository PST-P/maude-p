#!/bin/bash

FILES=""
NAME=$1
MAUDEFILE=$2
DIR=$3
OUTPUT=$4

DIR1="${DIR}PSrc"
if [[ -e $DIR1 ]]; then
  DIR1="${DIR}PSrc/*.p"
  for file in $(ls $DIR1); do
    FILES+=" $file"  
  done
fi

DIR1="${DIR}PSpec"
if [[ -e $DIR1 ]]; then
  DIR1="${DIR}PSpec/*.p"
  for file in $(ls $DIR1); do
    FILES+=" $file"  
  done
fi

DIR1="${DIR}PTst"
if [[ -e $DIR1 ]]; then
  DIR1="${DIR}PTst/*.p"
  for file in $(ls $DIR1); do
    FILES+=" $file"  
  done
fi

cat $FILES | python3 parser.py $NAME $MAUDEFILE $DIR > $OUTPUT
