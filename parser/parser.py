"""
File: parser.py
Author: Carlos Ramírez

"""

from lark import Lark
from sys import *
import datetime as dt

"""
Specification of P Grammar in Lark
"""
grammar = Lark(r'''
	?start: program

	?program: topdecl*                                                                      -> program

	?topdecl: typedecl                                                                      -> typedecl
		    	| eventdecl                                                                     -> eventdecl
		    	| machinedecl                                                                   -> machinedecl
		    	| funcdecl                                                                      -> funcdecl
		    	| testdecl                                                                      -> testdecl
		    	| moduledecl                                                                    -> moduledecl
		    	| specdecl                                                                      -> specdecl

	?typedecl: "type" IDENTIFIER "=" type ";"                                               -> userdefinedtype
	         | "type" IDENTIFIER ";"                                                        -> foreigntype
	         | "enum" IDENTIFIER "{" IDENTIFIER (","  IDENTIFIER)* "}"                      -> enumtype1
	         | "enum" IDENTIFIER "{" numberedenumelem ("," numberedenumelem)* "}"           -> enumtype2

	?eventdecl: "event" IDENTIFIER (":" type)? ";"                                          -> event

	?machinedecl: "machine" IDENTIFIER machinebody                                          -> machine

	?funcdecl: "fun" IDENTIFIER "(" funparamlist? ")" (":" type)? functionbody              -> funcdecl1
	         | "fun" IDENTIFIER "(" funparamlist? ")" (":" type)? ";"                       -> funcdecl2

	?testdecl: "test" IDENTIFIER "[" "main" "=" IDENTIFIER "]" ":" modeexpr ";"             -> test1

	?moduledecl: "module" IDENTIFIER "=" modeexpr ";"                                       -> namedmoddecl

	?specdecl: "spec" IDENTIFIER "observes" IDENTIFIER ("," IDENTIFIER)* machinebody        -> spec

	?machinebody: "{" (vardecl | statedecl | funcdecl )* "}"                                -> machinebody

	?vardecl: "var" IDENTIFIER ("," IDENTIFIER)* ":" type ";"                               -> var

	?statedecl: startdecl IDENTIFIER "{" statebody* "}"                                     -> state

	?startdecl: "start" statetype? "state"                                                  -> start1
	          | statetype? "state"                                                          -> start2

	?statetype: "hot"                                                                       -> hotstate
	          | "cold"                                                                      -> coldstate

	?statebody: "entry" anonfunction                                                        -> entry1
	          | "entry" IDENTIFIER ";"                                                      -> entry2
	          | "exit" functionbody                                                         -> entry3
	          | "exit" IDENTIFIER ";"                                                       -> entry4
	          | "defer" IDENTIFIER ("," IDENTIFIER)* ";"                                    -> entry5
	          | "ignore" IDENTIFIER ("," IDENTIFIER)* ";"                                   -> entry6
	          | "on" IDENTIFIER ("," IDENTIFIER)* "do" anonfunction                         -> entry7
	          | "on" IDENTIFIER ("," IDENTIFIER)* "do" IDENTIFIER ";"                       -> entry8
	          | "on" IDENTIFIER ("," IDENTIFIER)* "goto" IDENTIFIER ";"                     -> entry9
	          | "on" IDENTIFIER ("," IDENTIFIER)* "goto" IDENTIFIER "with" anonfunction     -> entry10
	          | "on" IDENTIFIER ("," IDENTIFIER)* "goto" IDENTIFIER "with" IDENTIFIER ";"   -> entry11

	?anonfunction: ("(" funparamlist ")")? functionbody                                     -> anonfunc

	?funparamlist: IDENTIFIER ":" type ("," IDENTIFIER ":" type)*                           -> funcparamlist

	?functionbody: "{" vardecl* statement* "}"                                              -> funcbody

	?statement: lvalue "=" expression ";"                                                   -> assignstm
	          | lvalue "+=" "(" expression "," expression ")" ";"                           -> insertstm
	          | lvalue "+=" "(" expression ")" ";"                                          -> addstm
	          | lvalue "-=" expression ";"                                                  -> removestm
	          | "break" ";"                                                                 -> breakstm
	          | "continue" ";"                                                              -> continuestm
	          | "return" expression? ";"                                                    -> returnstm
	          | "assert" expression ("," expression)? ";"                                   -> assertstm
	          | "print" expression ";"                                                      -> printstm
	          | "foreach" "(" IDENTIFIER "in" expression ")" "{" statement+ "}"             -> foreachstm
	          | "while" "(" expression ")" "{" statement+ "}"                               -> whilestm
	          | "if" "(" expression ")" blockstm ("else" blockstm)?                         -> ifstm
	          | "new" IDENTIFIER "(" expression? ")" ";"                                    -> ctorstm
	          | IDENTIFIER "("  ")" ";"                                                     -> funcallstm1
	          | IDENTIFIER "(" expression ("," expression)* ")" ";"                         -> funcallstm2
	          | "raise" expression ("," expression)? ";"                                    -> raisestm
	          | "send" expression "," expression ("," expression)? ";"                      -> sendstm
	          | "announce" expression ("," expression)? ";"                                 -> announcestm
	          | "goto" IDENTIFIER ("," expression)? ";"                                     -> gotostm
	          | "receive" "{" revcase+ "}"                                                  -> receivestm

  ?blockstm: "{" statement+ "}"                                                           -> blockstm1
           | statement                                                                    -> blockstm2

	?expression: FLOAT                                                                      -> floatexp
	           | INT                                                                        -> intexp
			       | BOOL                                                                       -> boolexp
			       | "$"                                                                        -> choiceexp
   	         | "halt"                                                                     -> haltexp
   	         | "this"                                                                     -> thisexp
	           | "null"                                                                     -> nullexp
   	         | ESCAPED_STRING                                                             -> stringexp
   	         | IDENTIFIER                                                                 -> varexp
	           | "(" expression ")"                                                         -> parentexp
	           | "choose" "(" expression? ")"                                               -> chooseexp
	           | "default" "(" type ")"                                                     -> defaultexp
	           | "-" expression                                                             -> unaryexp1
	           | "!" expression                                                             -> unaryexp2
	           | expression operatorbin expression                                          -> binaryexp
	           | expression "in" expression                                                 -> containsexp
	           | expression "[" expression "]"                                              -> accessexp
	           | "keys" "(" expression ")"                                                  -> keysexp
	           | "values" "(" expression ")"                                                -> valuesexp
	           | "sizeof" "(" expression ")"                                                -> sizeofexp
	           | tupleaccessexp                                                             -> tupleaccessexp
	           | expression "as" type                                                       -> castexp
	           | expression "to" type                                                       -> coerceexp 
	           | "(" tuplebody ")"                                                          -> tupleexp
	           | "(" namedtuplebody ")"                                                     -> namedtupleexp
	           | "new" IDENTIFIER "(" ")"                                                   -> newexp1
	           | "new" IDENTIFIER "(" expression ("," expression)* ")"                      -> newexp2
	           | IDENTIFIER "(" ")"                                                         -> funcallexp1
	           | IDENTIFIER "(" expression ("," expression)* ")"                            -> funcallexp2

	?tupleaccessexp . 4 : expression "." INT                                                -> tupleaccessexp
	                    | expression "." IDENTIFIER                                         -> namedtupleaccessexp

	?modeexpr: "(" modeexpr ")"                                                             -> anonmodeexp
	         | "{" bindexpr ("," bindexpr)* "}"                                             -> primmodeexp
	         | "union" modeexpr ("," modeexpr)+                                             -> unionmodeexp
	         | "assert" IDENTIFIER ("," IDENTIFIER)* "in" modeexpr                          -> assertmodeexp
	         | IDENTIFIER                                                                   -> idmodeexp

	?bindexpr: IDENTIFIER                                                                   -> bind1
	         | IDENTIFIER "->" IDENTIFIER                                                   -> bind2 

  ?tuplebody: expression ("," expression)*                                                -> tuplebody

  ?namedtuplebody: IDENTIFIER "=" expression ("," IDENTIFIER "=" expression)* (",")?      -> namedtuplebody

  ?operatorbin: "+"                                                                       -> add
   	          | "-"                                                                       -> subs
   	          | "/"                                                                       -> div
   	          | "*"                                                                       -> mult
              | "=="                                                                      -> equal
   	          | "!="                                                                      -> neq
   	          | "&&"                                                                      -> and
   	          | "||"                                                                      -> or
   	          | "<"                                                                       -> less
   	          | ">"                                                                       -> great
   	          | "<="                                                                      -> leq
   	          | ">="                                                                      -> geq

	?lvalue: IDENTIFIER                                                                     -> lvalueid
	       | lvalue "." IDENTIFIER                                                          -> namedtuplefield
	       | lvalue "." INT                                                                 -> tuplefield
	       | lvalue "[" expression "]"                                                      -> collectionlookup

	?rvalue: expression                                                                     -> rvalueexp

	?revcase: "case" IDENTIFIER ("," IDENTIFIER)* ":" anonfunction                          -> revcase

	?numberedenumelem: IDENTIFIER "=" INT                                                   -> numberedelem

	?type: "int"                                                                            -> inttype
	  	 | "bool"                                                                           -> booltype
		   | "float"                                                                          -> floattype
	  	 | "string"                                                                         -> stringtype
		   | "event"                                                                          -> eventtype
	  	 | "machine"                                                                        -> machinetype
		   | "(" type ("," type)* ")"                                                         -> tupletype
		   | "(" IDENTIFIER ":" type ("," IDENTIFIER  ":" type)* ")"                          -> namedtupletype
		   | "seq" "[" type  "]"                                                              -> seqtype
	  	 | "set" "[" type  "]"                                                              -> settype
	  	 | "map" "[" type "," type  "]"                                                     -> maptype
		   | "data"                                                                           -> datatype
	  	 | "any"                                                                            -> anytype
		   | IDENTIFIER                                                                       -> userdefinedtype
 
	IDENTIFIER: (LETTER | "_") (LETTER | DIGIT | "_" | "-")* 

	BOOL: "true" | "false"

	COMMENT: "//" /[^\n]/* | "/" ("*")+ /[^*]/* ("*")+ "/"
	%import common.WORD
	%import common.DIGIT
  %import common.SIGNED_FLOAT
  %import common.INT
  %import common.FLOAT
  %import common.USIGNED_INT
  %import common.LCASE_LETTER
  %import common.UCASE_LETTER
  %import common.ESCAPED_STRING
  %import common.LETTER
  %import common.WS
  %ignore WS
  %ignore COMMENT

    ''')

module_name, maude_file, pproject = argv[1], argv[2], argv[3]

"""
Auxiliary Structures:

- mapIdSort: this map relates each basic type with the corresponding Maude sort in the VarId hierarchy
- mapUserSort: this map relates each user defined type or machine with the sort in the VarId hierarchy (MachVarId or VarId)
- mapIdType: this map relates each basic type with the corresponding Maude type
- mapUserType: this map relates each user defined type with the corresponding Maude type
- mapOp: this map relates the label of each binary operator with the corresponding Maude operator 
- varsCnt: this map relates each variable in the P program to the Maude Sort associated to its first ocurrence. This map is used
           to determine whether a variable must be renamed.
- declCons: this map relates each Maude constant with its sort. It is used to check whether the constant has been previously declared.
- mapParams: this map mantains the association between each machine and function and the original names and the aliases for its parameters,
             in particular the name of the fields for named tuples.
- mapTypesDef: this map relates the names of user defined types with its definition and facilitates the parsing of code involving user
               defined types.

"""
mapIdSort = {"inttype" : "IntVarId", "floattype" : "FloatVarId", "booltype" : "BoolVarId", "stringtype" : "StringVarId",
             "maptype" : "MapVarId", "settype" : "SetVarId", "seqtype" : "SeqVarId", "namedtupletype" : "VarId", "machinetype" : "MachVarId",
             "tupletype" : "VarId", "anytype" : "VarId", "eventtype" : "VarId"}
mapIdType = {"inttype" : "Int", "floattype" : "Float", "booltype" : "Bool", "stringtype" : "String"}
mapOp = {"add" : "+", "subs" : "-", "mult" : "*", "div" : "/", "equal" : "==", "neq" : "=/=", "and" : "&&", "or" : "||", "less" : "<",
         "great" : ">", "leq" : "<=", "geq" : ">="}
mapUserSort, mapUserType, varsCnt, declCons, mapParams, mapTypesDef = {}, {}, {}, {}, {}, {}

"""
Operation parse(pgm, ans, varsId):
  - pgm is the AST for the P program to parse
  - file is the list in which the Maude program lines will be stored
  - varsId is the map with the correspondence between the variables in the P program and the variables in the Maude Program

This operation parses the P program given in the AST pgm.

"""
def parse(pgm, file, varsId):
  prev = -1
  if pgm.data == 'program':
  	for td in pgm.children:
  	  if td.data == "machinedecl":
  	  	file.append('')
  	  	file.append('  --- Constants for machine {}'.format(td.children[0].children[0]))
  	  	processMachineDecl(td, file, varsId)
  	  elif td.data == "funcdecl":
  	  	file.append('')
  	  	file.append('  --- Constants for function {}'.format(td.children[0].children[0]))
  	  	processFunDecl(td, file, varsId)
  	  elif td.data == "testdecl":
  	  	file.append('')
  	  	file.append('  --- Constants for test {}'.format(td.children[0].children[0]))
  	  	processTestDecl(td, file, varsId)
  	  elif td.data == 'moduledecl':
  	  	file.append('')
  	  	file.append('  --- Constants for module {}'.format(td.children[0].children[0]))
  	  	processModuleDecl(td, file)
  	  elif td.data == 'specdecl':
  	  	file.append('')
  	  	file.append('  --- Constants for monitor {}'.format(td.children[0].children[0]))
  	  	processSpecDecl(td, file, varsId)

"""
Operation processTypeDecls(td, file, varsId):
  - tree is the AST for the P program to parse
  - file is the list in which the Maude program lines will be stored
  - varsId is the map with the correspondence between the variables in the P program and the constants in the Maude Program

This operation parses the P type declarations in the P program given.

"""
def processTypeDecls(tree, file, varsId):
	l, flag = tree.find_data('typedecl'), True
	for e in l:
		if flag:
			file.append('  --- Constants for type declarations')
			flag = False
		processTypeDeclAux(e, file, varsId)
	file.append('')

"""
Operation processTypeDeclAux(td, file, varsId):
  - td is the the tree for the type declaration to parse
  - file is the list in which the sorts and equations of the Maude program lines will be stored
  - varsId is the map with the correspondence between the variables in the P program and the constants in the Maude Program

This operation parses the P type declaration in the tree td given.

"""
def processTypeDeclAux(td, file, varsId):
  name, decltype, types = varsId[str(td.children[0].children[0])], td.children[0].data, []
  if decltype == "userdefinedtype":
    content = td.children[0].children[1]
    file.append('  sort {} .'.format(name))
    file.append('  subsort {} < UserDefinedType .'.format(name))
    file.append('  op {} : -> UserDefinedTypeName .'.format(name))
    if content.data == "namedtupletype":
    	mapTypesDef[name] = content
    	i = 0
    	while i < len(content.children):
    		field = str(content.children[i])
    		if content.children[i + 1].data != "userdefinedtype":
    			sort = mapIdSort[content.children[i + 1].data]
    			types.append(mapIdType[content.children[i + 1].data])
    		else:
    			sort = mapUserSort[content.children[i + 1].children[0]]
    			types.append(mapUserType[content.children[i + 1].children[0]])
    		if field not in declCons:
    			declCons[field] = sort
    			file.append('  op {} : -> {} .'.format(field, sort))
    		i += 2
    	file.append('  op {} : {} -> {} .'.format(name, ' '.join(types), name))
    elif content.data == 'userdefinedtype':
    	mapTypesDef[name] = mapTypesDef[content.children[0]]
    else:
    	mapTypesDef[name] = content
  elif decltype == 'foreigntype':
  	file.append('  sort {} .'.format(name))
  	file.append('  subsort {} < UserDefinedType .'.format(name))
  	file.append('  op {} : -> UserDefinedTypeName .'.format(name))
  elif decltype == "enumtype1":
  	content, A = td.children[0].children, []
  	file.append('  sort {} .'.format(name))
  	file.append('  subsort {} < UserDefinedType .'.format(name))
  	file.append('  op {} : -> UserDefinedTypeName .'.format(name))

  	for cnst in content[1:]:
  		cnst = str(cnst)
  		if cnst not in varsCnt or varsCnt[cnst] == name: varsCnt[cnst], varsId[cnst] = name, convertName(cnst)
  		A.append(varsId[cnst])
  	file.append('  ops {} : -> {} .'.format(' '.join(A), name))
  elif decltype == 'enumtype2':
  	file.append('  sort {} .'.format(name))

"""
Operation processEventDecls(tree, file, varsId):
  - tree is the tree of the event declaration to parse
  - file is the list in which the Maude program lines will be stored
  - varsId is the map with the correspondence between the variables in the P program and the constants in the Maude Program

This operation parses the P event declaration given in the tree.

"""
def processEventDecls(tree, file, varsId):
	l, flag = tree.find_data('event'), True
	for e in l:
		if flag:
			file.append('  --- Constants for event declarations')
			flag = False
		name = str(e.children[0])
		varsCnt[name], varsId[name] = "EventName", convertName(name)
		file.append('  op {} : -> EventName .'.format(varsId[name]))

		if len(e.children) == 1: mapParams[name] = [varsId[name], {}]
		else:
			tp, d = e.children[1], dict()
			if tp.data == "namedtupletype" or (tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef):
				tp = collapseType(tp)
				processTypeAux(tp, d, file)
				mapParams[name] = [varsId[name], {"input" : ["input", d]}]
			else:
				mapParams[name] = [varsId[name], d]

"""
Operation processMachineDecl(mach, file, varsId):
  - mach is the tree of the machine declaration to parse
  - file is the list in which the Maude program lines will be stored
  - varsId is the map with the correspondence between the variables in the P program and the constants in the Maude Program for
    the context (environment) associated to the machine declaration.

This operation parses the P machine declaration given in the AST mach.

"""
def processMachineDecl(mach, file, varsId):
  eq, name, entries = [], varsId[str(mach.children[0].children[0])], mach.children[0].children[1].children
  processMachineBody(name, entries, 'MachineDecl', 'machine {}'.format(name), file, eq, varsId)
  file.extend(eq)

"""
Operation processSpecDecl(spec, file, varsId):
  - spec is the tree of the specification declaration to parse
  - file is the list in which the Maude program lines will be stored
  - varsId is the map with the correspondence between the variables in the P program and the constants in the Maude Program for
    the context (environment) associated to the specification declaration.

This operation parses the P specification declaration given in the AST spec.

"""
def processSpecDecl(spec, file, varsId):
  name, entries = varsId[str(spec.children[0].children[0])], spec.children[0].children[-1].children
  eq, obs = [], [convertName(var) for var in spec.children[0].children[1:-1]]
  processMachineBody(name, entries, 'SpecDecl', 'spec {} observes {}'.format(name, ', '.join(obs)), file, eq, varsId)
  file.extend(eq)

"""
Operation processTestDecl(test, file, varsId):
  - test is the tree of the test declaration to parse
  - file is the list in which the sorts and equations of the Maude program lines will be stored
  - varsId is the map with the correspondence between the variables in the P program and the constants in the Maude Program for
    the context (environment) associated to the test declaration.

This operation parses the P specification declaration given in the AST test.

"""
def processTestDecl(test, file, varsId):
  name, name2 = test.children[0].children[0], test.children[0].children[1]
  registerVariable(name, name, "test", "test", varsId)
  #if name not in varsCnt or varsCnt[name] == "test": varsCnt[name], varsId[name] = "test", name
  #else: varsId[name] = "%s$%s" % (name, "test")

  name, name2 = varsId[name], varsId[name2]
  mapUserSort[name] = "TestDecl"
  file.append('  op {}# : -> TestDecl .'.format(name))
  file.append('  op {} : -> Name .'.format(name))
  file.append('  eq {}# = test {} [main = {}] :'.format(name, name, name2))
  file.append('    {} ; .'.format(processModExpr(test.children[0].children[2])))

"""
Operation processModuleDecl(mod, file):
  - mod is the tree of the module declaration to parse
  - file is the list in which the sorts and equations of the Maude program lines will be stored

This operation parses the P module declaration given in the AST mod.

"""
def processModuleDecl(mod, file):
	name = mod.children[0].children[0]
	file.append('  op {} : -> Name .'.format(name))
	file.append('  --- module {} = {} ; .'.format(name, processModExpr(mod.children[0].children[1])))

"""
Operation processStateDecl(entry, file, eq, varsId, mapParams, mach):
  - entry is the AST of the state declaration.
  - file is the list in which the sorts and equations of the Maude program lines will be stored
  - eq is the list in which is generated the code of the state that must be added in the respective Maude equation. This is
    necessary in order to avoid unnecessary traversal in the AST.
  - varsId is the map with the correspondence between the variables in the P program and the constants in the Maude Program for
    the context (environment) associated to the state declaration.
  - mapParams is the dictionary in where is stored the functions and machine functions and entries and their respective names of
    parameters.
  - mach is a string which stands for the name of the machine aasociated to the state declaration

This operation parses the P state declaration given in the AST entry.

"""
def processStateDecl(entry, file, eq, varsId, mapParams, mach):
  name = varsId[str(entry.children[1])]
  if len(entry.children[0].children) == 0: tstate = "state"
  else: tstate = "hot state" if (entry.children[0].children[0].data == 'hotstate') else "cold state"
  
  eq.append('')
  if entry.children[0].data == "start1": eq.append('       start {} {} {{'.format(tstate, name))
  else: eq.append('       {} {} {{'.format(tstate, name))

  i, entries = 0, entry.children[2:]
  while i < len(entries):
    ent = entries[i]
    if ent.data == 'entry1':
  	  fun = ent.children[0]
  	  body, copyVars = fun.children[-1].children, dict(varsId)

  	  if len(fun.children) == 1: eq.append('         entry {')
  	  else:
  	  	params = fun.children[0].children
  	  	eq.append('         entry ({}) {{'.format(', '.join(processParams(params, copyVars, file, mapParams[mach][1][name][1]['entry']))))
  	  processVars(copyVars, body, file, eq)
  	  eq.extend(processBody(body, '         ', copyVars, file))
  	  eq.append('         }')
    elif ent.data == 'entry2':
  	  fun = ent.children[0]
  	  eq.append('         entry {} ;'.format(fun))
    elif ent.data == 'entry3':
  	  body, copyVars = ent.children[0].children, dict(varsId)
  	  eq.append('         exit {')
  	  processVars(copyVars, body, file, eq)
  	  eq.extend(processBody(body, '         ', copyVars, file))
  	  eq.append('         }')
    elif ent.data == 'entry4':
  	  fun = ent.children[0]
  	  eq.append('         exit {} ;'.format(fun))
    elif ent.data == 'entry5':
  	  eq.append('         defer {} ;'.format(', '.join([mapParams[e][0] for e in ent.children])))
    elif ent.data == 'entry6':
  	  eq.append('         ignore {} ;'.format(', '.join([mapParams[e][0] for e in ent.children])))
    elif ent.data == 'entry7':
  	  fun = ent.children[-1]
  	  body, copyVars = fun.children[-1].children, dict(varsId)

  	  if len(fun.children) == 1:
  	    eq.append("         on {} do {{".format(', '.join([mapParams[e][0] for e in ent.children[:-1]])))
  	  else:
  	  	params, ev = fun.children[0].children, ent.children[0]
  	  	if params[1].data == 'namedtupletype' or (params[1].data == "userdefinedtype" and params[1].children[0] in mapTypesDef):
  	  	  mapParams[ev][1][str(params[0])] = [str(params[0]), mapParams[ev][1]["input"][1]]
  	  	eq.append("         on {} do ({}) {{".format(', '.join([mapParams[e][0] for e in ent.children[:-1]]), ', '.join(processParams(params, copyVars, file, mapParams[ev][1]))))
  	  processVars(copyVars, body, file, eq)
  	  eq.extend(processBody(body, '         ', copyVars, file))
  	  eq.append('         }')
    elif ent.data == 'entry8':
  	  fun = ent.children[-1]
  	  eq.append('         on {} do {} ;'.format(', '.join([mapParams[e][0] for e in ent.children[:-1]]), fun))
    elif ent.data == 'entry9':
  	  #st = ent.children[-1]
  	  st = varsId[ent.children[-1]]
  	  eq.append('         on {} goto {} ;'.format(', '.join([mapParams[e][0] for e in ent.children[:-1]]), st))
    elif ent.data == 'entry10':
  	  #fun, st = ent.children[-1], ent.children[-2]
  	  fun, st = ent.children[-1], varsId[ent.children[-2]]
  	  body, copyVars = fun.children[-1].children, dict(varsId)
  	  if len(fun.children) == 1:
  	    eq.append("         on {} goto {} with {{".format(', '.join([mapParams[e][0] for e in ent.children[:-2]]), st))
  	  else:	
  	  	params, ev = fun.children[0].children, ent.children[0]
  	  	eq.append("         on {} goto {} with ({}) {{".format(', '.join([mapParams[e][0] for e in ent.children[:-2]]), st, ', '.join(processParams(params, copyVars, file, mapParams[ev][1]))))
  	  processVars(copyVars, body, file, eq)
  	  eq.extend(processBody(body, '         ', copyVars, file))
  	  eq.append('        }')
    elif ent.data == 'entry11':
  	  #st, fun = ent.children[-2], ent.children[-1]
  	  st, fun = ent.children[-2], varsId[ent.children[-1]]
  	  eq.append('         on {} goto {} with {} ;'.format(', '.join([mapParams[e][0] for e in ent.children[:-2]]), st, fun))
    if i < len(entries) - 1: eq.append('')
    i += 1
  eq.append('       }')

"""
Operation processVarDecl(entry, varsId, file, eq, flag):
  - entry is the AST of the variable declaration
  - file is the list in which the sorts and equations of the Maude program lines will be stored
  - eq is the list in which is generated the code of the state that must be added in the respective Maude equation. This is
    necessary in order to avoid unnecessary traversal in the AST
  - varsId is the map with the correspondence between the variables in the P program and the constants in the Maude Program for
    the context (environment) associated to the variable declaration
  - flag is a boolean which indicates whether the P code must be added

This operation parses the P variable declaration given in the AST entry.

"""
def processVarDecl(entry, varsId, file, eq, flag):
	sort = getMaudeSort(entry.children[-1])
	for e in entry.children[:-1]:
		e = str(e)
		registerVariable(e, convertName(e), sort, getPrefix(sort), varsId)
		if entry.children[-1].data == 'namedtupletype' or (entry.children[-1].data == 'userdefinedtype' and entry.children[-1].children[0] in mapTypesDef):
			tpAux = collapseType(entry.children[-1])
			processVarsType(tpAux, e, varsId, file)
		if varsId[e] not in declCons:
			declCons[varsId[e]] = sort
			file.append('  op {} : -> {} .'.format(varsId[e], sort))
		if flag:
			tp = getIdTypeP(entry.children[-1], varsId[e], varsId)
			eq.append('       var {} : {} ;'.format(varsId[e], tp))

def processMachineBody(nameMach, entries, tp1, tp2, file, eq, varsId):
  file.append('  op {}# : -> {} .'.format(nameMach, tp1))
  file.append('  op {} : -> UserDefinedTypeName .'.format(nameMach))
  file.append('  op {} : -> Name .'.format(nameMach))
  eq.append('  eq {}# = {} {{'.format(nameMach, tp2))

  copyVars = dict(varsId)
  for entry in entries:
  	if entry.data == "var":
  		processVarDecl(entry, copyVars, file, eq, True)
  	elif entry.data == "state":
  	  name = str(entry.children[1])
  	  registerVariable(name, name, "Name", "State", copyVars)
  	  if copyVars[name] not in declCons:
  	  	declCons[copyVars[name]] = "Name"
  	  	file.append('  op {} : -> Name .'.format(copyVars[name]))
  	else:
  	  name = str(entry.children[0])
  	  registerVariable(name, convertName(name), "Name", "Name", copyVars)
  	  if copyVars[name] not in declCons:
  	  	declCons[copyVars[name]] = "Name"
  	  	file.append('  op {}# : -> FunDecl .'.format(copyVars[name]))
  	  	file.append('  op {} : -> Name .'.format(copyVars[name]))

  for entry in entries:
  	if entry.data == "state":
  	  processStateDecl(entry, file, eq, copyVars, mapParams, nameMach)
  	elif entry.data == 'funcdecl1':
  	  namefun, copyVars2, body = copyVars[str(entry.children[0])], dict(copyVars), entry.children[-1].children
  	  processVars(copyVars2, body, file, eq)
  	  processFunDeclMach(entry, eq, '     ', copyVars2, file, mapParams[nameMach][1][namefun][1])
  	  eq.append('')
  	elif entry.data == 'funcdecl2':
  	  namefun = copyVars[str(entry.children[0])]
  	  processFunDeclMach(entry, eq, '     ', copyVars, file, mapParams[nameMach][1][namefun][1])
  	  eq.append('')
  file.append('')
  eq.append('  } .')

def processFunDecl(fun, file, varsId):
  name, tmp, copyVars = varsId[str(fun.children[0].children[0])], [], dict(varsId)
  if name not in declCons:
  	declCons[varsId[name]] = "Name"
  	file.append('  op {}# : -> FunDecl .'.format(varsId[name]))
  	file.append('  op {} : -> Name .'.format(varsId[name]))
  
  if len(fun.children[0].children) > 1 and fun.children[0].children[1].data == 'funcparamlist': params = fun.children[0].children[1].children
  else: params = []

  if fun.children[0].data == 'funcdecl1':
  	body = fun.children[0].children[-1].children
  	processVars(copyVars, body, file, tmp)

  	if len(fun.children[0].children) == 2 or (len(fun.children[0].children) == 3 and fun.children[0].children[1].data == 'funcparamlist'):
  		tmp.append('  eq {}# = fun {}({}) {{'.format(name, name, ", ".join(processParams(params, copyVars, file, mapParams[name][1]))))
  	else:
  		tp = fun.children[0].children[-2]
  		if tp.data == 'namedtupletype' or (tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef):
  			#if tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef: tp = mapTypesDef[tp.children[0]]
  			tp = collapseType(tp)
  			processVarsType(tp, "", copyVars, file)
  		tp = getIdTypeP(tp, "", copyVars)
  		tmp.append('  eq {}# = fun {}({}) : {} {{'.format(name, name, ", ".join(processParams(params, copyVars, file, mapParams[name][1])), tp))
  	tmp.extend(processBody(body, '  ', copyVars, file))
  	tmp.append('  } .')
  else:
  	if len(fun.children[0].children) == 1 or (len(fun.children[0].children) == 2 and fun.children[0].children[1].data == 'funcparamlist'):
  		tmp.append('  eq {}# = fun {}({}) ; .'.format(name, name, ", ".join(processParams(params, copyVars, file, mapParams[name][1]))))
  	else:
  		tp = fun.children[0].children[-1]
  		if tp.data == 'namedtupletype' or (tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef):
  			tp = collapseType(tp)
  			#if tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef: tp = mapTypesDef[tp.children[0]]
  			processVarsType(tp, "", copyVars, file)
  		tp = getIdTypeP(tp, "", copyVars)
  		tmp.append('  eq {}# = fun {}({}) : {} ; .'.format(name, name, ", ".join(processParams(params, copyVars, file, mapParams[name][1])), tp))
  file.extend(tmp)

def processFunDeclMach(fun, tmp, sep, varsId, file, mapParams):
  name, copyVars = varsId[str(fun.children[0])], dict(varsId)

  if fun.data == 'funcdecl1': body, s, lim = fun.children[-1].children, 2, -2
  else: s, lim = 1, -1

  if fun.children[1].data == 'funcparamlist': params, s = fun.children[1].children, s + 1
  else: params = []

  if len(fun.children) == s:
    tmp.append('{}  fun {} ({}) {{'.format(sep, name, ", ".join(processParams(params, copyVars, file, mapParams))))
  else:
  	tp = fun.children[lim]
  	if tp.data == 'namedtupletype' or (tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef):
  		tp = collapseType(tp)
  		#if tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef: tp = mapTypesDef[tp.children[0]]
  		processVarsType(tp, "", copyVars, file)
  	tp = getIdTypeP(tp, name, copyVars)
  	tmp.append('{}  fun {} ({}) : {} {{'.format(sep, name, ", ".join(processParams(params, copyVars, file, mapParams)), tp))

  if fun.data == 'funcdecl1':
  	tmp.extend(processBody(body, '       ', copyVars, file))
  	tmp.append('{}  }}'.format(sep))

def processBody(body, sep, varsId, file):
	tmp, i = [], 0
	while i < len(body) and body[i].data == "var":
		for e in body[i].children[:-1]:
			e = varsId[str(e)]
			tp = getIdTypeP(body[i].children[-1], e, varsId)
			tmp.append('{}  var {} : {} ;'.format(sep, e, tp))
		i += 1
	while i < len(body):
		tmpStm = processStatement(body[i], sep, varsId, file)
		for l in tmpStm:
			tmp.append('  {}'.format(l))
		i += 1
	return tmp

def processVars(copyVars, body, file, eq):
  j = 0
  while j < len(body) and body[j].data == 'var':
  	var = body[j]
  	processVarDecl(var, copyVars, file, eq, False)
  	j += 1

def processVarsType(tpAux, var, varsId, file):
	i = 0
	while i < len(tpAux.children):
		namevar, tp = str(tpAux.children[i]), tpAux.children[i + 1]
		sort = getMaudeSort(tp)
		registerVariable(namevar, namevar, sort, getPrefix(sort), varsId)

		if var != "": varsId["%s.%s" % (var, namevar)] = "%s.%s" % (var, varsId[namevar])
		if varsId[namevar] not in declCons:
			declCons[varsId[namevar]] = sort
			file.append('  op {} : -> {} .'.format(varsId[namevar], sort))

		if tp.data == 'namedtupletype' or (tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef):
			tp = collapseType(tp)
			processVarsType(tp, "%s.%s" % (var, namevar), varsId, file)
		i += 2

def processStatement(stm, sep, varsId, file):
  ans = []
  if stm.data == 'breakstm': ans.append('{}break ;'.format(sep))
  elif stm.data == 'continuestm': ans.append('{}continue ;'.format(sep))
  elif stm.data == 'returnstm':
  	if len(stm.children) > 0: ans.append('{}return {} ;'.format(sep, parseExp(stm.children[0], varsId)))
  	else: ans.append('{}return ;'.format(sep))
  elif stm.data == 'assignstm':
  	lvalue, rvalue = processLValue(stm.children[0], varsId), parseExp(stm.children[1], varsId)
  	ans.append('{}{} = {} ;'.format(sep, lvalue, rvalue))
  elif stm.data == 'addstm' or stm.data == 'removestm':
  	lvalue, rvalue = processLValue(stm.children[0], varsId), parseExp(stm.children[1], varsId)
  	op = '+=' if stm.data == 'addstm' else '-='
  	ans.append('{}{} {} ( {} ) ;'.format(sep, lvalue, op, rvalue))
  elif stm.data == 'insertstm':
  	lvalue, expr, rvalue = processLValue(stm.children[0], varsId), parseExp(stm.children[1], varsId), parseExp(stm.children[2], varsId)
  	ans.append('{}{} += ( {}, {} ) ;'.format(sep, lvalue, expr, rvalue))
  elif stm.data == 'assertstm':
  	if len(stm.children) == 2:
  	  ans.append('{}assert {}, {} ;'.format(sep, parseExp(stm.children[0], varsId), parseExp(stm.children[1], varsId)))
  	else:
  	  ans.append('{}assert {}, "" ;'.format(sep, parseExp(stm.children[0], varsId)))
  elif stm.data == 'printstm':
  	ans.append('{}print {} ;'.format(sep, parseExp(stm.children[0], varsId)))
  elif stm.data == 'foreachstm':
  	ans.append('{}foreach ({} in {}) {{'.format(sep, stm.children[0], parseExp(stm.children[1], varsId)))
  	for j in range(2, len(stm.children)):
  	  ans.extend(processStatement(stm.children[j], sep + '  ', varsId, file))
  	ans.append('{}}}'.format(sep))
  elif stm.data == 'whilestm':
  	ans.append('{}while ({}) {{'.format(sep, parseExp(stm.children[0], varsId)))
  	for j in range(1, len(stm.children)):
  	  ans.extend(processStatement(stm.children[j], sep + '  ', varsId, file))
  	ans.append('{}}}'.format(sep))
  elif stm.data == 'ifstm':
  	ans.append('{}if ({}) {{'.format(sep, parseExp(stm.children[0], varsId)))
  	for j in range(len(stm.children[1].children)):
  	  ans.extend(processStatement(stm.children[1].children[j], sep + '  ', varsId, file))
  	ans.append('{}}}'.format(sep))
  	if(len(stm.children) > 2):
  	  ans.append('{}else{{'.format(sep))
  	  for j in range(len(stm.children[2].children)):
  	    ans.extend(processStatement(stm.children[2].children[j], sep + '  ', varsId, file))
  	  ans.append('{}}}'.format(sep))
  elif stm.data == 'ctorstm':
  	if len(stm.children) == 1: ans.append('{}new {} ( ) ;'.format(sep, stm.children[0]))
  	else: ans.append('{}{} ;'.format(sep, parseNewExp(stm, varsId)))
  elif stm.data == 'funcallstm1':
  	name = varsId[stm.children[0]]
  	ans.append("{}{}() ;".format(sep, name))
  elif stm.data == 'funcallstm2':
  	i, A, name = 1, [], varsId[stm.children[0]]
  	while i < len(stm.children):
  	  A.append(parseExp(stm.children[i], varsId))
  	  i += 1
  	ans.append("{}{}({}) ;".format(sep, name, ', '.join(A)))
  elif stm.data == 'raisestm':
  	if len(stm.children) == 1: ans.append('{}raise {} ;'.format(sep, parseExp(stm.children[0], varsId)))
  	else: ans.append('{}raise {}, {} ;'.format(sep, parseExp(stm.children[0], varsId), parseExp(stm.children[1], varsId)))
  elif stm.data == 'sendstm':
  	if len(stm.children) == 2:
  	  ans.append('{}send {}, {} ;'.format(sep, parseExp(stm.children[0], varsId), parseExp(stm.children[1], varsId)))
  	else:
  	  ev = parseExp(stm.children[1], varsId)
  	  ans.append('{}send {}, {}, {} ;'.format(sep, parseExp(stm.children[0], varsId), ev, parseSendExp(stm.children[2], varsId, ev)))
  elif stm.data == 'announcestm':
  	if len(stm.children) == 1: ans.append('{}announce {} ;'.format(sep, parseExp(stm.children[0], varsId)))
  	else: ans.append('{}announce {}, {} ;'.format(sep, parseExp(stm.children[0], varsId), parseExp(stm.children[1], varsId)))
  elif stm.data == 'gotostm':
  	name = varsId[stm.children[0]]
  	if len(stm.children) == 1: ans.append('{}goto {} ;'.format(sep, name))
  	else: ans.append('{}goto {}, {} ;'.format(sep, name, parseExp(stm.children[1], varsId)))
  elif stm.data == 'receivestm':
  	ans.append('{}receive {{'.format(sep))
  	for case in stm.children:
  	  evs = [mapParams[e][0] for e in case.children[:-1]]
  	  fun = case.children[-1]
  	  body = fun.children[-1].children
  	  copyVars = dict(varsId)
  	  if len(fun.children) == 1:
  	    ans.append('{}  case {} : {{'.format(sep, ', '.join(evs)))
  	  else:
  	  	params = fun.children[0].children
  	  	if params[1].data == 'namedtupletype' or (params[1].data == "userdefinedtype" and params[1].children[0] in mapTypesDef):
  	  	  mapParams[evs[0]][1][str(params[0])] = [str(params[0]), mapParams[evs[0]][1]["input"][1]]
  	  	ans.append('{}  case {} : ({}) {{'.format(sep, ', '.join(evs), ', '.join(processParams(params, copyVars, file, mapParams[evs[0]][1]))))
  	  processVars(copyVars, body, file, ans)
  	  ans.extend(processBody(body, '           ', copyVars, file))
  	  ans.append('{}  }}'.format(sep))
  	ans.append('{}}}'.format(sep))
  return ans

def parseExp(exp, varsId):
  ans = "X"
  if exp.data == "floatexp": ans = exp.children[0]
  elif exp.data == "intexp": ans = exp.children[0]
  elif exp.data == "boolexp": ans = exp.children[0]
  elif exp.data == "stringexp": ans = exp.children[0]
  elif exp.data == "nullexp": ans = "null"
  elif exp.data == "haltexp": ans = "halt"
  elif exp.data == "thisexp": ans = "this"
  elif exp.data == "choiceexp": ans = "$"
  elif exp.data == "varexp":
  	var = str(exp.children[0])
  	if var in varsId: ans = varsId[var]
  	else: ans = var
  elif exp.data == 'parentexp':
  	ans = "( {} )".format(parseExp(exp.children[0], varsId))
  elif exp.data == 'unaryexp1':
  	ans = "- {}".format(parseExp(exp.children[0], varsId))
  elif exp.data == 'unaryexp2':
  	ans = "! {}".format(parseExp(exp.children[0], varsId))
  elif exp.data == 'binaryexp':
  	ans = "{} {} {}".format(parseExp(exp.children[0], varsId), mapOp[exp.children[1].data], parseExp(exp.children[2], varsId))
  elif exp.data == 'containsexp':
  	ans = "{} in {}".format(parseExp(exp.children[0], varsId), parseExp(exp.children[1], varsId))
  elif exp.data == 'accessexp':
  	ans = "{}[{}]".format(parseExp(exp.children[0], varsId), parseExp(exp.children[1], varsId))
  elif exp.data == 'keysexp':
  	ans = "keys ( {} )".format(parseExp(exp.children[0], varsId))
  elif exp.data == 'valuesexp':
  	ans = "values ( {} )".format(parseExp(exp.children[0], varsId))
  elif exp.data == 'sizeofexp':
  	ans = "sizeof ( {} )".format(parseExp(exp.children[0], varsId))
  elif exp.data == 'tupleaccessexp':
  	taccessexp = exp.children[0]
  	res = parseTupleAccessExp(exp, varsId)
  	aux = ".".join(res.split("@"))
  	if aux in varsId: ans = " . ".join(varsId[aux].split("."))
  	else: ans = " . ".join(res.split("@"))
  elif exp.data == 'chooseexp':
  	if len(exp.children) > 0: ans = "choose ( {} )".format(parseExp(exp.children[0], varsId))
  	else: ans = "choose ()"
  elif exp.data == 'castexp':
  	ans = "{} as {}".format(parseExp(exp.children[0], varsId), getIdTypeP(exp.children[1], "", varsId))
  elif exp.data == 'coerceexp':
  	ans = "{} to {}".format(parseExp(exp.children[0], varsId), getIdTypeP(exp.children[1], "", varsId))
  elif exp.data == 'defaultexp':
  	ans = "default ( {} )".format(getIdTypeP(exp.children[0], "", varsId))
  elif exp.data == 'tupleexp':
  	items = exp.children[0].children
  	A = [parseExp(exp, varsId) for exp in items], 
  	ans = "( {} )".format(', '.join(A))
  elif exp.data == 'namedtupleexp':
  	items, A, i = exp.children[0].children, [], 0
  	while i < len(items):
  	  A.append('{} = {}'.format(items[i], parseExp(items[i + 1], varsId)))
  	  i += 2
  	ans = "( {} )".format(', '.join(A))
  elif exp.data == 'newexp1':
  	ans = "new {} ( )".format(varsId[exp.children[0]])
  elif exp.data == 'newexp2':
  	ans = parseNewExp(exp, varsId)
  elif exp.data == 'funcallexp1':
  	ans = "{}()".format(varsId[str(exp.children[0])])
  elif exp.data == 'funcallexp2':
  	i, A = 1, []
  	while i < len(exp.children):
  	  A.append(parseExp(exp.children[i], varsId))
  	  i += 1
  	ans = "{}({})".format(str(exp.children[0]), ', '.join(A))
  return ans

def parseTupleAccessExp(exp, varsId):
	taccessexp = exp.children[0]
	if taccessexp.data == 'namedtupleaccessexp':
		if taccessexp.children[0].data == 'varexp': ans = "%s@%s" % (taccessexp.children[0].children[0], taccessexp.children[1])
		elif taccessexp.children[0].data == 'tupleaccessexp': ans = "%s@%s" % (parseTupleAccessExp(taccessexp.children[0], varsId), taccessexp.children[1])
		else: ans = "%s@%s" % (parseExp(taccessexp.children[0], varsId), taccessexp.children[1])
	else:
		if taccessexp.children[0].data == 'varexp': ans = "%s@%d" % (varsId[taccessexp.children[0].children[0]], taccessexp.children[1])
		elif taccessexp.children[0].data == "tupleaccessexp": ans = "%s@%d" % (parseTupleAccessExp(taccessexp.children[0], varsId), taccessexp.children[1])
		else: ans = "%s@%d" % (parseExp(taccessexp.children[0], varsId), taccessexp.children[1])
	return ans

def processParams(params, varsId, file, mapParams):
  i, lparams = 0, []
  while i < len(params):
  	namevar = str(params[i])
  	processParamsType(namevar, params[i + 1], varsId, file, mapParams)
  	lparams.append('{} : {}'.format(varsId[namevar], getIdTypeP(params[i + 1], namevar, varsId)))
  	i += 2
  return lparams

def processParamsType(namevar, tp, varsId, file, mapParams):
	sort, pos = getMaudeSort(tp), namevar.rfind(".")
	var = namevar[pos + 1:]
	tp = collapseType(tp)
	if var not in mapParams:
		sort = getMaudeSort(tp)
		if var not in varsCnt or varsCnt[var] == sort: varsCnt[var], aux = sort, var
		else: aux = "%s$%s" % (var, getPrefix(sort))
		mapParams[var] = [aux, {}]

	varsId[namevar] = convertName(mapParams[var][0])
	if pos != -1: varsId[namevar] = "%s.%s" % (varsId[namevar[:pos]], mapParams[var][0])

	if mapParams[var][0]  not in declCons:
		declCons[mapParams[var][0]] = sort
		file.append('  op {} : -> {} .'.format(mapParams[var][0], sort))

	if tp.data == 'namedtupletype'or (tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef):
		i, tp = 0, collapseType(tp)
		while i < len(tp.children):
			processParamsType("%s.%s" % (namevar, tp.children[i]), tp.children[i + 1], varsId, file, mapParams[var][1])
			i += 2

def processTypeAux(tp, mapParams, file):
	i = 0
	while i < len(tp.children):
	  field, tpAux = str(tp.children[i]), tp.children[i + 1]
	  sort = getMaudeSort(tpAux)

	  if field not in varsCnt or varsCnt[field] == sort: varsCnt[field], aux = sort, field
	  else: aux = "%s$%s" % (field, getPrefix(sort))

	  mapParams[field] = [aux, {}]
	  if aux not in declCons:
	  	declCons[aux] = sort
	  	file.append('  op {} : -> {} .'.format(mapParams[field][0], sort))
	  if tpAux.data == "namedtupletype" or (tpAux.data == 'userdefinedtype' and tpAux.children[0] in mapTypesDef):
	  	tpAux = collapseType(tpAux)
	  	processTypeAux(tpAux, mapParams[field][1], file)
	  i += 2
	return tp

def processParamsAux(params, varsId, mapParams):
  i = 0
  while i < len(params):
  	namevar = str(params[i])
  	processParamsTypeAux(namevar, params[i + 1], varsId, mapParams)
  	i += 2

def processParamsTypeAux(namevar, tp, varsId, mapParams):
	sort, var = getMaudeSort(tp), namevar.split(".")
	if var[-1] not in varsCnt or varsCnt[var[-1]] == sort: varsCnt[var[-1]], aux = sort, convertName(var[-1])
	else: aux = "%s$%s" % (convertName(var[-1]), getPrefix(sort))

	mapParams[var[-1]] = [aux, {}]
	if tp.data == 'namedtupletype' or (tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef):
		i, tp = 0, collapseType(tp)
		while i < len(tp.children):
			processParamsTypeAux("%s.%s" % (namevar, tp.children[i]), tp.children[i + 1], varsId, mapParams[var[-1]][1])
			i += 2

def parseSendExp(exp, varsId, ev):
  if exp.data != "namedtupleexp": ans = parseExp(exp, varsId)
  else:
  	items, A, i = exp.children[0].children, [], 0
  	while i < len(items):
  	  A.append('{} = {}'.format(mapParams[ev][1]["input"][1][items[i]][0], parseExp(items[i + 1], varsId)))
  	  i += 2
  	ans = "({})".format(', '.join(A))
  return ans

def parseNewExp(exp, varsId):
	params, mach = [], varsId[exp.children[0]]
	if exp.children[1].data == "namedtupleexp":
		items, A, j = exp.children[1].children[0].children, [], 0
		while j < len(items):
			A.append('{} = {}'.format(mapParams[mach][1][mapParams[mach][2]][1]["entry"]["input"][1][items[j]][0], parseExp(items[j + 1], varsId)))
			j += 2
		params.append('({})'.format(', '.join(A)))
	else:
		params.append(parseExp(exp.children[1], varsId))
	ans = "new {}({})".format(mach, ', '.join(params))
	return ans

def processLValue(lval, varsId):
  if lval.data == 'collectionlookup': ans = "{}[{}]".format(processLValue(lval.children[0], varsId), parseExp(lval.children[1], varsId))
  elif lval.data == 'namedtuplefield': ans = "{} . {}".format(processLValue(lval.children[0], varsId), varsId[lval.children[1]])
  elif lval.data == 'tuplefield': ans = "{} . {}".format(processLValue(lval.children[0], varsId), lval.children[1])
  else: ans = varsId[lval.children[0]]
  return ans

def processModExpr(mod):
  ans = ''
  if mod.data == 'anonmodeexp': ans = '( {} )'.format(processModExpr(mod.children[0]))
  elif mod.data == 'idmodeexp': ans = mod.children[0]
  elif mod.data == 'primmodeexp':
  	A = [processBindExpr(bind) for bind in mod.children]
  	ans = '{{ {} }}'.format(', '.join(A))
  elif mod.data == 'unionmodeexp':
  	A = [processModExpr(mod) for mod in mod.children]
  	ans = 'union {}'.format(', '.join(A))
  elif mod.data == 'assertmodeexp':
  	A = [var for var in mod.children[:-1]]
  	ans = 'assert {} in {}'.format(', '.join(A), processModExpr(mod.children[-1]))
  return ans

def getIdTypeP(type, var, varsId):
  ans = ''
  if type.data == 'inttype': ans = 'int'
  elif type.data == 'floattype': ans = 'float'
  elif type.data == 'booltype': ans = 'bool'
  elif type.data == 'stringtype': ans = 'string'
  elif type.data == 'eventtype': ans = 'event'
  elif type.data == 'machinetype': ans = 'machine'
  elif type.data == 'datatype': ans = 'data'
  elif type.data == 'anytype': ans = 'any'
  elif type.data == 'userdefinedtype':
  	namevar = str(type.children[0])
  	ans = varsId[namevar]
  elif type.data == 'seqtype': ans = 'seq[{}]'.format(getIdTypeP(type.children[0], var, varsId))
  elif type.data == 'settype': ans = 'set[{}]'.format(getIdTypeP(type.children[0], var, varsId))
  elif type.data == 'maptype': ans = 'map[{},{}]'.format(getIdTypeP(type.children[0], var, varsId), getIdTypeP(type.children[1], var, varsId))
  elif type.data == 'namedtupletype':
  	ans, i = [], 0
  	while i < len(type.children):
  		if var != "":
  			tok = varsId["%s.%s" %(var, type.children[i])]
  			name, nvar = tok.split(".")[-1], "%s.%s" % (var, tok)
  		else:
  			name, nvar = varsId[type.children[i]], varsId[type.children[i]]
  		ans.append('{} : {}'.format(name, getIdTypeP(type.children[i + 1], nvar, varsId)))
  		i += 2
  	ans = '({})'.format(', '.join(ans))
  elif type.data == 'tupletype':
  	ans, i = [], 0
  	while i < len(type.children):
  	  ans.append('{}'.format(getIdTypeP(type.children[i], var, varsId)))
  	  i += 1
  	ans = '({})'.format(', '.join(ans))
  return ans

def processBindExpr(bind):
  if bind.data == 'bind1': ans = bind.children[0]
  else: ans = '{} -> {}'.format(bind.children[0], bind.children[1])
  return ans

def getPrefix(tp):
  if tp == "VarId": ans = "Var"
  else: ans = tp[:-5]
  return ans

def registerVariable(name, alias, sort, tp, varsId):
	if name not in varsCnt or varsCnt[name] == sort: varsCnt[name], varsId[name] = sort, alias
	else: varsId[name] = "%s$%s" % (alias, tp)

def getMaudeSort(tp):
	if tp.data != "userdefinedtype": sort = mapIdSort[tp.data]
	else: sort = mapUserSort[tp.children[0]]
	return sort

def collapseType(tp):
	ans = mapTypesDef[tp.children[0]] if tp.data == 'userdefinedtype' and tp.children[0] in mapTypesDef else tp
	return ans

def typeExp(exp, env, varsId):
  ans = "error"
  if exp.data == 'floatexp': ans = "float"
  elif exp.data == 'intexp': ans = "int"
  elif exp.data == 'boolexp': ans = "bool"
  elif exp.data == 'halexp': ans = "event"
  elif exp.data == 'thisexp': ans = "machine"
  elif exp.data == 'nullexp': ans = "any"
  elif exp.data == 'choiceexp': ans = "bool"
  elif exp.data == 'stringexp': ans = "string"
  elif exp.data == 'varexp': ans = env[exp.children[0]]
  elif exp.data == 'parentexp': ans = typeExp(exp.children[0], env, varsId)
  elif exp.data == 'chooseexp': 
  	if len(exp.children) > 0:
  	  t = typeExp(exp.children[0], env, varsId)
  	  if t == "int": ans = t
  	  else: ans = t[4:-1]
  	else:
  	  ans = "bool"
  elif exp.data == 'defaultexp': ans = getIdTypeP(exp.children[0], "", varsId)
  elif exp.data == 'unaryexp1': ans = typeExp(exp.children[0], env, varsId)
  elif exp.data == 'unaryexp2': ans = "bool"
  elif exp.data == 'binaryexp':
  	if exp.children[1].data in ("add", "div", "mult", "subs"):
  	  t1, t2 = typeExp(exp.children[0], env, varsId), typeExp(exp.children[2], env, varsId) 
  	  if t1 == "float" or t2 == "float": ans = "float"
  	  else: ans= "int"
  	else: ans = "bool"
  elif exp.data == 'tupleaccessexp':
  	t = typeExp(exp.children[0].children[0], env, varsId)
  	ans = t[4:-1]
  elif exp.data == 'accessexp':
  	t = typeExp(exp.children[0], env, varsId)
  	if t[:3] == "set" or t[:3] == "seq": ans = t[4:-1]
  	else: ans = t[4:-1].split(",")[1].strip()
  elif exp.data == 'keysexp':
  	t = typeExp(exp.children[0], env, varsId)
  	ans = t[4:-1].split(",")[0].strip()
  elif exp.data == 'valuesexp':
  	t = typeExp(exp.children[0], env, varsId)
  	ans = t[4:-1].split(",")[1].strip()
  elif exp.data == 'sizeofexp': ans = "int"
  elif exp.data == 'castexp':
  	ans = getIdTypeP(exp.children[1], "", varsId)
  elif exp.data == 'coerceexp':
  	ans = "int"
  elif exp.data == 'tupleexp':
  	items = exp.children[0].children
  	A = []
  	for exp in items:
  		A.append(typeExp(exp.children[0], env, varsId))
  	ans = "( {} )".format(', '.join(A))
  elif exp.data == 'namedtupleexp':
  	items = exp.children[0].children
  	A, i = [], 0
  	while i < len(items):
  	  A.append('{} = {}'.format(items[i], typeExp(items[i + 1].children[0], env, varsId)))
  	  i += 2
  	ans = "( {} )".format(', '.join(A))
  elif exp.data == 'newexp1' or exp.data == "newexp2": ans = "machine"
  elif exp.data == 'funcallexp1' or exp.data == 'funcallexp2':
  	ans = env[5:-1]
  return ans

def convertName(name):
	name = list(name)
	if name[0] == '_': name[0] = '#'
	elif name[-1] == '_': name[-1] = '#'
	for i in range(1, len(name) - 1):
		if name[i] == '_': name[i] = '-'
	name = ''.join(name)
	return name

def getNamesMachines(tree, varsId):
	l = tree.find_data('machine')
	for e in l:
		name = varsId[str(e.children[0])]
		entries = e.children[1].children
		for entry in entries:
			if entry.data == 'funcdecl1' or entry.data == 'funcdecl2':
				namefunc = str(entry.children[0])
				mapParams[name][1][namefunc] = [namefunc, {}]
				if entry.children[1].data == 'funcparamlist':
					params = entry.children[1].children
					processParamsAux(params, varsId, mapParams[name][1][namefunc][1])
			elif entry.data == 'state':
				nameState = str(entry.children[1])
				if entry.children[0].data == 'start1':
					mapParams[name][2] = nameState
				mapParams[name][1][nameState] = [nameState, {}]
				i, body, flag = 0, entry.children[2:], True
				while i < len(body) and flag:
					if body[i].data == "entry1":
						d = {}
						if body[i].children[0].children[0].data == 'funcparamlist':
							params = body[i].children[0].children[0].children
							processParamsAux(params, varsId, d)

							mapParams[name][1][nameState][1]["entry"] = d
							if params[i] != "input":
								mapParams[name][1][nameState][1]["entry"]["input"] = mapParams[name][1][nameState][1]["entry"][params[i]]
						flag = False
					i += 1

def registerElements(tree, varsId):
	l = tree.find_data('machine')
	for e in l:
		name = str(e.children[0])
		mapUserSort[name], mapUserType[name], varsCnt[name], varsId[name] = "MachVarId", "MachId", "MachVarId", convertName(name)
		mapParams[name] = [varsId[name], {}, None]

	l = tree.find_data('spec')
	for e in l:
		name = str(e.children[0])
		mapUserSort[name], mapUserType[name], varsCnt[name], varsId[name] = "MachVarId", "MachId", "MachVarId", convertName(name)
		mapParams[name] = [varsId[name], {}, None]

	l = tree.find_data('typedecl')
	for e in l:
		name = str(e.children[0].children[0])
		mapUserSort[name], mapUserType[name], varsCnt[name], varsId[name] = "VarId", name, "VarId", convertName(name)

	l = tree.find_data('funcdecl')
	for func in l:
		nameFunc = str(func.children[0].children[0])
		mapUserSort[nameFunc], varsCnt[nameFunc], varsId[nameFunc] = "FuncDecl", "FuncDecl", convertName(nameFunc)
		mapParams[nameFunc] = [varsId[nameFunc], {}]
		if len(func.children[0].children) > 1 and func.children[0].children[1].data == 'funcparamlist':
			params = func.children[0].children[1].children
			processParamsAux(params, varsId, mapParams[nameFunc][1])

def generate():
  global mapUserSort, mapUserType, varsCnt, mapParams, mapTypesDef
  file = []
  file.append('--------------------------------------------------------------------')
  file.append('---- Maude Code Generated for Module {}'.format(module_name))
  file.append('---- ')
  file.append('---- Located at {}'.format(pproject))
  file.append('---- Date {}'.format(dt.datetime.now()))
  file.append('---- ')
  file.append('--------------------------------------------------------------------')
  file.append('')
  file.append('load {}'.format(maude_file))
  file.append('')
  file.append('mod {} is'.format(module_name))
  file.append('  inc SYSTEM-EXEC .')
  file.append('')

  lines = stdin.readlines()
  program = ''.join(lines)
  tree, varsId = grammar.parse(program), {}

  registerElements(tree, varsId)
  processTypeDecls(tree, file, varsId)
  processEventDecls(tree, file, varsId)
  getNamesMachines(tree, varsId)
  parse(tree, file, varsId)

  file.append('endm')  

  print('\n'.join(file))

def generateRun():
	file = []
	file.append('')
	file.append('mod {}-RUN is'.format(module_name))
	file.append('  inc {} .'.format(module_name))
	file.append('')
	file.append('  op init : -> System .')
	machs, ini, i = [], '  eq init = init(', 0
	for e in mapUserSort:
		if mapUserSort[e] == "MachVarId" or mapUserSort[e] == "FuncDecl" or mapUserSort[e] == "TestDecl":
			if i == 0: file.append('{}[{}#]'.format(ini, e))
			else: file.append('                 [{}#]'.format(e))
			i += 1
	file[-1] = ('{}) .'.format(file[-1]))
	file.append('endm')
	print('\n'.join(file))

generate()
generateRun()