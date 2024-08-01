using Microsoft.Coyote;
using Microsoft.Coyote.Actors;
using Microsoft.Coyote.Runtime;
using Microsoft.Coyote.Specifications;
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Plang.CSharpRuntime;
using Plang.CSharpRuntime.Values;
using Plang.CSharpRuntime.Exceptions;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 162, 219, 414, 1998
namespace PImplementation
{
}
namespace PImplementation
{
    internal partial class eUpdateQuery : PEvent
    {
        public eUpdateQuery() : base() {}
        public eUpdateQuery (PrtNamedTuple payload): base(payload){ }
        public override IPrtValue Clone() { return new eUpdateQuery();}
    }
}
namespace PImplementation
{
    internal partial class eReadQuery : PEvent
    {
        public eReadQuery() : base() {}
        public eReadQuery (PrtNamedTuple payload): base(payload){ }
        public override IPrtValue Clone() { return new eReadQuery();}
    }
}
namespace PImplementation
{
    internal partial class eReadQueryResp : PEvent
    {
        public eReadQueryResp() : base() {}
        public eReadQueryResp (PrtNamedTuple payload): base(payload){ }
        public override IPrtValue Clone() { return new eReadQueryResp();}
    }
}
namespace PImplementation
{
    internal partial class eWithDrawReq : PEvent
    {
        public eWithDrawReq() : base() {}
        public eWithDrawReq (PrtNamedTuple payload): base(payload){ }
        public override IPrtValue Clone() { return new eWithDrawReq();}
    }
}
namespace PImplementation
{
    internal partial class eWithDrawResp : PEvent
    {
        public eWithDrawResp() : base() {}
        public eWithDrawResp (PrtNamedTuple payload): base(payload){ }
        public override IPrtValue Clone() { return new eWithDrawResp();}
    }
}
namespace PImplementation
{
    internal partial class eSpec_BankBalanceIsAlwaysCorrect_Init : PEvent
    {
        public eSpec_BankBalanceIsAlwaysCorrect_Init() : base() {}
        public eSpec_BankBalanceIsAlwaysCorrect_Init (PrtMap payload): base(payload){ }
        public override IPrtValue Clone() { return new eSpec_BankBalanceIsAlwaysCorrect_Init();}
    }
}
namespace PImplementation
{
    public static partial class GlobalFunctions
    {
        public static async Task<PrtInt> ReadBankBalance(PMachineValue database, PrtInt accountId, PMachine currentMachine)
        {
            PrtInt currentBalance = ((PrtInt)0);
            PMachineValue TMP_tmp0 = null;
            PEvent TMP_tmp1 = null;
            PrtInt TMP_tmp2 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp3 = (new PrtNamedTuple(new string[]{"accountId"},((PrtInt)0)));
            PrtInt TMP_tmp4 = ((PrtInt)0);
            PrtInt TMP_tmp5 = ((PrtInt)0);
            TMP_tmp0 = (PMachineValue)(((PMachineValue)((IPrtValue)database)?.Clone()));
            TMP_tmp1 = (PEvent)(new eReadQuery((new PrtNamedTuple(new string[]{"accountId"},((PrtInt)0)))));
            TMP_tmp2 = (PrtInt)(((PrtInt)((IPrtValue)accountId)?.Clone()));
            TMP_tmp3 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"accountId"}, TMP_tmp2)));
            currentMachine.TrySendEvent(TMP_tmp0, (Event)TMP_tmp1, TMP_tmp3);
            var PGEN_recvEvent = await currentMachine.TryReceiveEvent(typeof(eReadQueryResp), typeof(PHalt));
            switch (PGEN_recvEvent) {
                case PHalt _hv: { currentMachine.TryRaiseEvent(_hv); break;} 
                case eReadQueryResp PGEN_evt: {
                    PrtNamedTuple resp = (PrtNamedTuple)(PGEN_evt.Payload);
                    TMP_tmp4 = (PrtInt)(((PrtNamedTuple)resp)["balance"]);
                    TMP_tmp5 = (PrtInt)(((PrtInt)((IPrtValue)TMP_tmp4)?.Clone()));
                    currentBalance = TMP_tmp5;
                } break;
            }
            return ((PrtInt)((IPrtValue)currentBalance)?.Clone());
        }
    }
}
namespace PImplementation
{
    public static partial class GlobalFunctions
    {
        public static void UpdateBankBalance(PMachineValue database_1, PrtInt accId, PrtInt bal, PMachine currentMachine)
        {
            PMachineValue TMP_tmp0_1 = null;
            PEvent TMP_tmp1_1 = null;
            PrtInt TMP_tmp2_1 = ((PrtInt)0);
            PrtInt TMP_tmp3_1 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp4_1 = (new PrtNamedTuple(new string[]{"accountId","balance"},((PrtInt)0), ((PrtInt)0)));
            TMP_tmp0_1 = (PMachineValue)(((PMachineValue)((IPrtValue)database_1)?.Clone()));
            TMP_tmp1_1 = (PEvent)(new eUpdateQuery((new PrtNamedTuple(new string[]{"accountId","balance"},((PrtInt)0), ((PrtInt)0)))));
            TMP_tmp2_1 = (PrtInt)(((PrtInt)((IPrtValue)accId)?.Clone()));
            TMP_tmp3_1 = (PrtInt)(((PrtInt)((IPrtValue)bal)?.Clone()));
            TMP_tmp4_1 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"accountId","balance"}, TMP_tmp2_1, TMP_tmp3_1)));
            currentMachine.TrySendEvent(TMP_tmp0_1, (Event)TMP_tmp1_1, TMP_tmp4_1);
        }
    }
}
namespace PImplementation
{
    public static partial class GlobalFunctions
    {
        public static PrtMap CreateRandomInitialAccounts(PrtInt numAccounts, PMachine currentMachine)
        {
            PrtInt i = ((PrtInt)0);
            PrtMap bankBalance = new PrtMap();
            PrtBool TMP_tmp0_2 = ((PrtBool)false);
            PrtBool TMP_tmp1_2 = ((PrtBool)false);
            PrtInt TMP_tmp2_2 = ((PrtInt)0);
            PrtInt TMP_tmp3_2 = ((PrtInt)0);
            PrtInt TMP_tmp4_2 = ((PrtInt)0);
            while (((PrtBool)true))
            {
                TMP_tmp0_2 = (PrtBool)((i) < (numAccounts));
                TMP_tmp1_2 = (PrtBool)(((PrtBool)((IPrtValue)TMP_tmp0_2)?.Clone()));
                if (TMP_tmp1_2)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp2_2 = (PrtInt)(((PrtInt)currentMachine.TryRandom(((PrtInt)100))));
                TMP_tmp3_2 = (PrtInt)((TMP_tmp2_2) + (((PrtInt)10)));
                ((PrtMap)bankBalance)[i] = TMP_tmp3_2;
                TMP_tmp4_2 = (PrtInt)((i) + (((PrtInt)1)));
                i = TMP_tmp4_2;
            }
            return ((PrtMap)((IPrtValue)bankBalance)?.Clone());
        }
    }
}
namespace PImplementation
{
    public static partial class GlobalFunctions
    {
        public static void SetupClientServerSystem(PrtInt numClients, PMachine currentMachine)
        {
            PrtInt i_1 = ((PrtInt)0);
            PMachineValue server = null;
            PrtSeq accountIds = new PrtSeq();
            PrtMap initAccBalance = new PrtMap();
            PrtInt TMP_tmp0_3 = ((PrtInt)0);
            PrtMap TMP_tmp1_3 = new PrtMap();
            PrtMap TMP_tmp2_3 = new PrtMap();
            PMachineValue TMP_tmp3_3 = null;
            PrtSeq TMP_tmp4_3 = new PrtSeq();
            PrtInt TMP_tmp5_1 = ((PrtInt)0);
            PrtBool TMP_tmp6 = ((PrtBool)false);
            PrtBool TMP_tmp7 = ((PrtBool)false);
            PMachineValue TMP_tmp8 = null;
            PrtInt TMP_tmp9 = ((PrtInt)0);
            PrtInt TMP_tmp10 = ((PrtInt)0);
            PrtInt TMP_tmp11 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp12 = (new PrtNamedTuple(new string[]{"serv","accountId","balance"},null, ((PrtInt)0), ((PrtInt)0)));
            PrtInt TMP_tmp13 = ((PrtInt)0);
            TMP_tmp0_3 = (PrtInt)(((PrtInt)((IPrtValue)numClients)?.Clone()));
            TMP_tmp1_3 = (PrtMap)(GlobalFunctions.CreateRandomInitialAccounts(TMP_tmp0_3, currentMachine));
            initAccBalance = TMP_tmp1_3;
            TMP_tmp2_3 = (PrtMap)(((PrtMap)((IPrtValue)initAccBalance)?.Clone()));
            TMP_tmp3_3 = (PMachineValue)(currentMachine.CreateInterface<I_BankServer>( currentMachine, TMP_tmp2_3));
            server = TMP_tmp3_3;
            currentMachine.Announce((Event)new eSpec_BankBalanceIsAlwaysCorrect_Init(new PrtMap()), initAccBalance);
            TMP_tmp4_3 = (PrtSeq)((initAccBalance).CloneKeys());
            accountIds = TMP_tmp4_3;
            while (((PrtBool)true))
            {
                TMP_tmp5_1 = (PrtInt)(((PrtInt)(accountIds).Count));
                TMP_tmp6 = (PrtBool)((i_1) < (TMP_tmp5_1));
                TMP_tmp7 = (PrtBool)(((PrtBool)((IPrtValue)TMP_tmp6)?.Clone()));
                if (TMP_tmp7)
                {
                }
                else
                {
                    break;
                }
                TMP_tmp8 = (PMachineValue)(((PMachineValue)((IPrtValue)server)?.Clone()));
                TMP_tmp9 = (PrtInt)(((PrtSeq)accountIds)[i_1]);
                TMP_tmp10 = (PrtInt)(((PrtSeq)accountIds)[i_1]);
                TMP_tmp11 = (PrtInt)(((PrtMap)initAccBalance)[TMP_tmp10]);
                TMP_tmp12 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"serv","accountId","balance"}, TMP_tmp8, TMP_tmp9, TMP_tmp11)));
                currentMachine.CreateInterface<I_Client>(currentMachine, TMP_tmp12);
                TMP_tmp13 = (PrtInt)((i_1) + (((PrtInt)1)));
                i_1 = TMP_tmp13;
            }
        }
    }
}
namespace PImplementation
{
    internal partial class BankServer : PMachine
    {
        private PMachineValue database_2 = null;
        public class ConstructorEvent : PEvent{public ConstructorEvent(PrtMap val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((PrtMap)value); }
        public BankServer() {
            this.sends.Add(nameof(eReadQuery));
            this.sends.Add(nameof(eReadQueryResp));
            this.sends.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.sends.Add(nameof(eUpdateQuery));
            this.sends.Add(nameof(eWithDrawReq));
            this.sends.Add(nameof(eWithDrawResp));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eReadQuery));
            this.receives.Add(nameof(eReadQueryResp));
            this.receives.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.receives.Add(nameof(eUpdateQuery));
            this.receives.Add(nameof(eWithDrawReq));
            this.receives.Add(nameof(eWithDrawResp));
            this.receives.Add(nameof(PHalt));
            this.creates.Add(nameof(I_Database));
        }
        
        public void Anon(Event currentMachine_dequeuedEvent)
        {
            BankServer currentMachine = this;
            PrtMap initialBalance = (PrtMap)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PMachineValue TMP_tmp0_4 = null;
            PrtMap TMP_tmp1_4 = new PrtMap();
            PrtNamedTuple TMP_tmp2_4 = (new PrtNamedTuple(new string[]{"server","initialBalance"},null, new PrtMap()));
            PMachineValue TMP_tmp3_4 = null;
            TMP_tmp0_4 = (PMachineValue)(currentMachine.self);
            TMP_tmp1_4 = (PrtMap)(((PrtMap)((IPrtValue)initialBalance)?.Clone()));
            TMP_tmp2_4 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"server","initialBalance"}, TMP_tmp0_4, TMP_tmp1_4)));
            TMP_tmp3_4 = (PMachineValue)(currentMachine.CreateInterface<I_Database>( currentMachine, TMP_tmp2_4));
            database_2 = TMP_tmp3_4;
            currentMachine.TryGotoState<WaitForWithdrawRequests>();
            return;
        }
        public async Task Anon_1(Event currentMachine_dequeuedEvent)
        {
            BankServer currentMachine = this;
            PrtNamedTuple wReq = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt currentBalance_1 = ((PrtInt)0);
            PrtNamedTuple response = (new PrtNamedTuple(new string[]{"status","accountId","balance","rId"},((PrtInt)0), ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            PMachineValue TMP_tmp0_5 = null;
            PrtInt TMP_tmp1_5 = ((PrtInt)0);
            PrtInt TMP_tmp2_5 = ((PrtInt)0);
            PrtInt TMP_tmp3_5 = ((PrtInt)0);
            PrtInt TMP_tmp4_4 = ((PrtInt)0);
            PrtBool TMP_tmp5_2 = ((PrtBool)false);
            PMachineValue TMP_tmp6_1 = null;
            PrtInt TMP_tmp7_1 = ((PrtInt)0);
            PrtInt TMP_tmp8_1 = ((PrtInt)0);
            PrtInt TMP_tmp9_1 = ((PrtInt)0);
            PrtInt TMP_tmp10_1 = ((PrtInt)0);
            PrtInt TMP_tmp11_1 = ((PrtInt)0);
            PrtInt TMP_tmp12_1 = ((PrtInt)0);
            PrtInt TMP_tmp13_1 = ((PrtInt)0);
            PrtInt TMP_tmp14 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp15 = (new PrtNamedTuple(new string[]{"status","accountId","balance","rId"},((PrtInt)0), ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            PrtInt TMP_tmp16 = ((PrtInt)0);
            PrtInt TMP_tmp17 = ((PrtInt)0);
            PrtInt TMP_tmp18 = ((PrtInt)0);
            PrtInt TMP_tmp19 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp20 = (new PrtNamedTuple(new string[]{"status","accountId","balance","rId"},((PrtInt)0), ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            PrtBool TMP_tmp21 = ((PrtBool)false);
            PMachineValue TMP_tmp22 = null;
            PMachineValue TMP_tmp23 = null;
            PEvent TMP_tmp24 = null;
            PrtNamedTuple TMP_tmp25 = (new PrtNamedTuple(new string[]{"status","accountId","balance","rId"},((PrtInt)0), ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            TMP_tmp0_5 = (PMachineValue)(((PMachineValue)((IPrtValue)database_2)?.Clone()));
            TMP_tmp1_5 = (PrtInt)(((PrtNamedTuple)wReq)["accountId"]);
            TMP_tmp2_5 = (PrtInt)(await GlobalFunctions.ReadBankBalance(TMP_tmp0_5, TMP_tmp1_5, currentMachine));
            currentBalance_1 = TMP_tmp2_5;
            TMP_tmp3_5 = (PrtInt)(((PrtNamedTuple)wReq)["amount"]);
            TMP_tmp4_4 = (PrtInt)((currentBalance_1) - (TMP_tmp3_5));
            TMP_tmp5_2 = (PrtBool)((TMP_tmp4_4) >= (((PrtInt)10)));
            if (TMP_tmp5_2)
            {
                TMP_tmp6_1 = (PMachineValue)(((PMachineValue)((IPrtValue)database_2)?.Clone()));
                TMP_tmp7_1 = (PrtInt)(((PrtNamedTuple)wReq)["accountId"]);
                TMP_tmp8_1 = (PrtInt)(((PrtNamedTuple)wReq)["amount"]);
                TMP_tmp9_1 = (PrtInt)((currentBalance_1) - (TMP_tmp8_1));
                GlobalFunctions.UpdateBankBalance(TMP_tmp6_1, TMP_tmp7_1, TMP_tmp9_1, currentMachine);
                TMP_tmp10_1 = (PrtInt)((PrtEnum.Get("WITHDRAW_SUCCESS")));
                TMP_tmp11_1 = (PrtInt)(((PrtNamedTuple)wReq)["accountId"]);
                TMP_tmp12_1 = (PrtInt)(((PrtNamedTuple)wReq)["amount"]);
                TMP_tmp13_1 = (PrtInt)((currentBalance_1) - (TMP_tmp12_1));
                TMP_tmp14 = (PrtInt)(((PrtNamedTuple)wReq)["rId"]);
                TMP_tmp15 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"status","accountId","balance","rId"}, TMP_tmp10_1, TMP_tmp11_1, TMP_tmp13_1, TMP_tmp14)));
                response = TMP_tmp15;
            }
            else
            {
                TMP_tmp16 = (PrtInt)((PrtEnum.Get("WITHDRAW_ERROR")));
                TMP_tmp17 = (PrtInt)(((PrtNamedTuple)wReq)["accountId"]);
                TMP_tmp18 = (PrtInt)(((PrtInt)((IPrtValue)currentBalance_1)?.Clone()));
                TMP_tmp19 = (PrtInt)(((PrtNamedTuple)wReq)["rId"]);
                TMP_tmp20 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"status","accountId","balance","rId"}, TMP_tmp16, TMP_tmp17, TMP_tmp18, TMP_tmp19)));
                response = TMP_tmp20;
            }
            TMP_tmp21 = (PrtBool)(((PrtBool)currentMachine.TryRandomBool()));
            if (TMP_tmp21)
            {
                TMP_tmp22 = (PMachineValue)(((PrtNamedTuple)wReq)["source"]);
                TMP_tmp23 = (PMachineValue)(((PMachineValue)((IPrtValue)TMP_tmp22)?.Clone()));
                TMP_tmp24 = (PEvent)(new eWithDrawResp((new PrtNamedTuple(new string[]{"status","accountId","balance","rId"},((PrtInt)0), ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)))));
                TMP_tmp25 = (PrtNamedTuple)(((PrtNamedTuple)((IPrtValue)response)?.Clone()));
                currentMachine.TrySendEvent(TMP_tmp23, (Event)TMP_tmp24, TMP_tmp25);
            }
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Init))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon))]
        class Init : State
        {
        }
        [OnEventDoAction(typeof(eWithDrawReq), nameof(Anon_1))]
        class WaitForWithdrawRequests : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Database : PMachine
    {
        private PMachineValue server_1 = null;
        private PrtMap balance = new PrtMap();
        public class ConstructorEvent : PEvent{public ConstructorEvent(PrtNamedTuple val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((PrtNamedTuple)value); }
        public Database() {
            this.sends.Add(nameof(eReadQuery));
            this.sends.Add(nameof(eReadQueryResp));
            this.sends.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.sends.Add(nameof(eUpdateQuery));
            this.sends.Add(nameof(eWithDrawReq));
            this.sends.Add(nameof(eWithDrawResp));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eReadQuery));
            this.receives.Add(nameof(eReadQueryResp));
            this.receives.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.receives.Add(nameof(eUpdateQuery));
            this.receives.Add(nameof(eWithDrawReq));
            this.receives.Add(nameof(eWithDrawResp));
            this.receives.Add(nameof(PHalt));
        }
        
        public void Anon_2(Event currentMachine_dequeuedEvent)
        {
            Database currentMachine = this;
            PrtNamedTuple input = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PMachineValue TMP_tmp0_6 = null;
            PMachineValue TMP_tmp1_6 = null;
            PrtMap TMP_tmp2_6 = new PrtMap();
            PrtMap TMP_tmp3_6 = new PrtMap();
            TMP_tmp0_6 = (PMachineValue)(((PrtNamedTuple)input)["server"]);
            TMP_tmp1_6 = (PMachineValue)(((PMachineValue)((IPrtValue)TMP_tmp0_6)?.Clone()));
            server_1 = TMP_tmp1_6;
            TMP_tmp2_6 = (PrtMap)(((PrtNamedTuple)input)["initialBalance"]);
            TMP_tmp3_6 = (PrtMap)(((PrtMap)((IPrtValue)TMP_tmp2_6)?.Clone()));
            balance = TMP_tmp3_6;
        }
        public void Anon_3(Event currentMachine_dequeuedEvent)
        {
            Database currentMachine = this;
            PrtNamedTuple query = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_7 = ((PrtInt)0);
            PrtBool TMP_tmp1_7 = ((PrtBool)false);
            PrtString TMP_tmp2_7 = ((PrtString)"");
            PrtInt TMP_tmp3_7 = ((PrtInt)0);
            PrtInt TMP_tmp4_5 = ((PrtInt)0);
            PrtInt TMP_tmp5_3 = ((PrtInt)0);
            TMP_tmp0_7 = (PrtInt)(((PrtNamedTuple)query)["accountId"]);
            TMP_tmp1_7 = (PrtBool)(((PrtBool)(((PrtMap)balance).ContainsKey(TMP_tmp0_7))));
            TMP_tmp2_7 = (PrtString)(((PrtString) String.Format("Invalid accountId received in the update query!")));
            currentMachine.TryAssert(TMP_tmp1_7,"Assertion Failed: " + TMP_tmp2_7);
            TMP_tmp3_7 = (PrtInt)(((PrtNamedTuple)query)["accountId"]);
            TMP_tmp4_5 = (PrtInt)(((PrtNamedTuple)query)["balance"]);
            TMP_tmp5_3 = (PrtInt)(((PrtInt)((IPrtValue)TMP_tmp4_5)?.Clone()));
            ((PrtMap)balance)[TMP_tmp3_7] = TMP_tmp5_3;
        }
        public void Anon_4(Event currentMachine_dequeuedEvent)
        {
            Database currentMachine = this;
            PrtNamedTuple query_1 = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_8 = ((PrtInt)0);
            PrtBool TMP_tmp1_8 = ((PrtBool)false);
            PrtString TMP_tmp2_8 = ((PrtString)"");
            PMachineValue TMP_tmp3_8 = null;
            PEvent TMP_tmp4_6 = null;
            PrtInt TMP_tmp5_4 = ((PrtInt)0);
            PrtInt TMP_tmp6_2 = ((PrtInt)0);
            PrtInt TMP_tmp7_2 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp8_2 = (new PrtNamedTuple(new string[]{"accountId","balance"},((PrtInt)0), ((PrtInt)0)));
            TMP_tmp0_8 = (PrtInt)(((PrtNamedTuple)query_1)["accountId"]);
            TMP_tmp1_8 = (PrtBool)(((PrtBool)(((PrtMap)balance).ContainsKey(TMP_tmp0_8))));
            TMP_tmp2_8 = (PrtString)(((PrtString) String.Format("Invalid accountId received in the read query!")));
            currentMachine.TryAssert(TMP_tmp1_8,"Assertion Failed: " + TMP_tmp2_8);
            TMP_tmp3_8 = (PMachineValue)(((PMachineValue)((IPrtValue)server_1)?.Clone()));
            TMP_tmp4_6 = (PEvent)(new eReadQueryResp((new PrtNamedTuple(new string[]{"accountId","balance"},((PrtInt)0), ((PrtInt)0)))));
            TMP_tmp5_4 = (PrtInt)(((PrtNamedTuple)query_1)["accountId"]);
            TMP_tmp6_2 = (PrtInt)(((PrtNamedTuple)query_1)["accountId"]);
            TMP_tmp7_2 = (PrtInt)(((PrtMap)balance)[TMP_tmp6_2]);
            TMP_tmp8_2 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"accountId","balance"}, TMP_tmp5_4, TMP_tmp7_2)));
            currentMachine.TrySendEvent(TMP_tmp3_8, (Event)TMP_tmp4_6, TMP_tmp8_2);
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Init_1))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon_2))]
        [OnEventDoAction(typeof(eUpdateQuery), nameof(Anon_3))]
        [OnEventDoAction(typeof(eReadQuery), nameof(Anon_4))]
        class Init_1 : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class Client : PMachine
    {
        private PMachineValue server_2 = null;
        private PrtInt accountId_1 = ((PrtInt)0);
        private PrtInt nextReqId = ((PrtInt)0);
        private PrtInt numOfWithdrawOps = ((PrtInt)0);
        private PrtInt currentBalance_2 = ((PrtInt)0);
        public class ConstructorEvent : PEvent{public ConstructorEvent(PrtNamedTuple val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((PrtNamedTuple)value); }
        public Client() {
            this.sends.Add(nameof(eReadQuery));
            this.sends.Add(nameof(eReadQueryResp));
            this.sends.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.sends.Add(nameof(eUpdateQuery));
            this.sends.Add(nameof(eWithDrawReq));
            this.sends.Add(nameof(eWithDrawResp));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eReadQuery));
            this.receives.Add(nameof(eReadQueryResp));
            this.receives.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.receives.Add(nameof(eUpdateQuery));
            this.receives.Add(nameof(eWithDrawReq));
            this.receives.Add(nameof(eWithDrawResp));
            this.receives.Add(nameof(PHalt));
        }
        
        public void Anon_5(Event currentMachine_dequeuedEvent)
        {
            Client currentMachine = this;
            PrtNamedTuple input_1 = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PMachineValue TMP_tmp0_9 = null;
            PMachineValue TMP_tmp1_9 = null;
            PrtInt TMP_tmp2_9 = ((PrtInt)0);
            PrtInt TMP_tmp3_9 = ((PrtInt)0);
            PrtInt TMP_tmp4_7 = ((PrtInt)0);
            PrtInt TMP_tmp5_5 = ((PrtInt)0);
            PrtInt TMP_tmp6_3 = ((PrtInt)0);
            PrtInt TMP_tmp7_3 = ((PrtInt)0);
            TMP_tmp0_9 = (PMachineValue)(((PrtNamedTuple)input_1)["serv"]);
            TMP_tmp1_9 = (PMachineValue)(((PMachineValue)((IPrtValue)TMP_tmp0_9)?.Clone()));
            server_2 = TMP_tmp1_9;
            TMP_tmp2_9 = (PrtInt)(((PrtNamedTuple)input_1)["balance"]);
            TMP_tmp3_9 = (PrtInt)(((PrtInt)((IPrtValue)TMP_tmp2_9)?.Clone()));
            currentBalance_2 = TMP_tmp3_9;
            TMP_tmp4_7 = (PrtInt)(((PrtNamedTuple)input_1)["accountId"]);
            TMP_tmp5_5 = (PrtInt)(((PrtInt)((IPrtValue)TMP_tmp4_7)?.Clone()));
            accountId_1 = TMP_tmp5_5;
            TMP_tmp6_3 = (PrtInt)((accountId_1) * (((PrtInt)100)));
            TMP_tmp7_3 = (PrtInt)((TMP_tmp6_3) + (((PrtInt)1)));
            nextReqId = TMP_tmp7_3;
            currentMachine.TryGotoState<WithdrawMoney>();
            return;
        }
        public void Anon_6(Event currentMachine_dequeuedEvent)
        {
            Client currentMachine = this;
            PrtInt index = ((PrtInt)0);
            PrtBool TMP_tmp0_10 = ((PrtBool)false);
            PMachineValue TMP_tmp1_10 = null;
            PEvent TMP_tmp2_10 = null;
            PMachineValue TMP_tmp3_10 = null;
            PrtInt TMP_tmp4_8 = ((PrtInt)0);
            PrtInt TMP_tmp5_6 = ((PrtInt)0);
            PrtInt TMP_tmp6_4 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp7_4 = (new PrtNamedTuple(new string[]{"source","accountId","amount","rId"},null, ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            PrtInt TMP_tmp8_3 = ((PrtInt)0);
            TMP_tmp0_10 = (PrtBool)((currentBalance_2) <= (((PrtInt)10)));
            if (TMP_tmp0_10)
            {
                currentMachine.TryGotoState<NoMoneyToWithDraw>();
                return;
            }
            TMP_tmp1_10 = (PMachineValue)(((PMachineValue)((IPrtValue)server_2)?.Clone()));
            TMP_tmp2_10 = (PEvent)(new eWithDrawReq((new PrtNamedTuple(new string[]{"source","accountId","amount","rId"},null, ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)))));
            TMP_tmp3_10 = (PMachineValue)(currentMachine.self);
            TMP_tmp4_8 = (PrtInt)(((PrtInt)((IPrtValue)accountId_1)?.Clone()));
            TMP_tmp5_6 = (PrtInt)(WithdrawAmount());
            TMP_tmp6_4 = (PrtInt)(((PrtInt)((IPrtValue)nextReqId)?.Clone()));
            TMP_tmp7_4 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"source","accountId","amount","rId"}, TMP_tmp3_10, TMP_tmp4_8, TMP_tmp5_6, TMP_tmp6_4)));
            currentMachine.TrySendEvent(TMP_tmp1_10, (Event)TMP_tmp2_10, TMP_tmp7_4);
            TMP_tmp8_3 = (PrtInt)((nextReqId) + (((PrtInt)1)));
            nextReqId = TMP_tmp8_3;
        }
        public void Anon_7(Event currentMachine_dequeuedEvent)
        {
            Client currentMachine = this;
            PrtNamedTuple resp_1 = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_11 = ((PrtInt)0);
            PrtBool TMP_tmp1_11 = ((PrtBool)false);
            PrtString TMP_tmp2_11 = ((PrtString)"");
            PrtInt TMP_tmp3_11 = ((PrtInt)0);
            PrtBool TMP_tmp4_9 = ((PrtBool)false);
            PrtInt TMP_tmp5_7 = ((PrtInt)0);
            PrtInt TMP_tmp6_5 = ((PrtInt)0);
            PrtString TMP_tmp7_5 = ((PrtString)"");
            PrtInt TMP_tmp8_4 = ((PrtInt)0);
            PrtInt TMP_tmp9_2 = ((PrtInt)0);
            PrtInt TMP_tmp10_2 = ((PrtInt)0);
            PrtBool TMP_tmp11_2 = ((PrtBool)false);
            PrtInt TMP_tmp12_2 = ((PrtInt)0);
            PrtInt TMP_tmp13_2 = ((PrtInt)0);
            PrtString TMP_tmp14_1 = ((PrtString)"");
            PrtInt TMP_tmp15_1 = ((PrtInt)0);
            PrtInt TMP_tmp16_1 = ((PrtInt)0);
            PrtString TMP_tmp17_1 = ((PrtString)"");
            PrtBool TMP_tmp18_1 = ((PrtBool)false);
            PrtInt TMP_tmp19_1 = ((PrtInt)0);
            PrtString TMP_tmp20_1 = ((PrtString)"");
            TMP_tmp0_11 = (PrtInt)(((PrtNamedTuple)resp_1)["balance"]);
            TMP_tmp1_11 = (PrtBool)((TMP_tmp0_11) >= (((PrtInt)10)));
            TMP_tmp2_11 = (PrtString)(((PrtString) String.Format("Bank balance must be greater than 10!!")));
            currentMachine.TryAssert(TMP_tmp1_11,"Assertion Failed: " + TMP_tmp2_11);
            TMP_tmp3_11 = (PrtInt)(((PrtNamedTuple)resp_1)["status"]);
            TMP_tmp4_9 = (PrtBool)((PrtValues.SafeEquals(PrtValues.Box((long) TMP_tmp3_11),PrtValues.Box((long) (PrtEnum.Get("WITHDRAW_SUCCESS"))))));
            if (TMP_tmp4_9)
            {
                TMP_tmp5_7 = (PrtInt)(((PrtNamedTuple)resp_1)["rId"]);
                TMP_tmp6_5 = (PrtInt)(((PrtNamedTuple)resp_1)["balance"]);
                TMP_tmp7_5 = (PrtString)(((PrtString) String.Format("Withdrawal with rId = {0} succeeded, new account balance = {1}",TMP_tmp5_7,TMP_tmp6_5)));
                PModule.runtime.Logger.WriteLine("<PrintLog> " + TMP_tmp7_5);
                TMP_tmp8_4 = (PrtInt)(((PrtNamedTuple)resp_1)["balance"]);
                TMP_tmp9_2 = (PrtInt)(((PrtInt)((IPrtValue)TMP_tmp8_4)?.Clone()));
                currentBalance_2 = TMP_tmp9_2;
            }
            else
            {
                TMP_tmp10_2 = (PrtInt)(((PrtNamedTuple)resp_1)["balance"]);
                TMP_tmp11_2 = (PrtBool)((PrtValues.SafeEquals(currentBalance_2,TMP_tmp10_2)));
                TMP_tmp12_2 = (PrtInt)(((PrtInt)((IPrtValue)currentBalance_2)?.Clone()));
                TMP_tmp13_2 = (PrtInt)(((PrtNamedTuple)resp_1)["balance"]);
                TMP_tmp14_1 = (PrtString)(((PrtString) String.Format("Withdraw failed BUT the account balance changed! client thinks: {0}, bank balance: {1}",TMP_tmp12_2,TMP_tmp13_2)));
                currentMachine.TryAssert(TMP_tmp11_2,"Assertion Failed: " + TMP_tmp14_1);
                TMP_tmp15_1 = (PrtInt)(((PrtNamedTuple)resp_1)["rId"]);
                TMP_tmp16_1 = (PrtInt)(((PrtNamedTuple)resp_1)["balance"]);
                TMP_tmp17_1 = (PrtString)(((PrtString) String.Format("Withdrawal with rId = {0} failed, account balance = {1}",TMP_tmp15_1,TMP_tmp16_1)));
                PModule.runtime.Logger.WriteLine("<PrintLog> " + TMP_tmp17_1);
            }
            TMP_tmp18_1 = (PrtBool)((currentBalance_2) > (((PrtInt)10)));
            if (TMP_tmp18_1)
            {
                TMP_tmp19_1 = (PrtInt)(((PrtInt)((IPrtValue)currentBalance_2)?.Clone()));
                TMP_tmp20_1 = (PrtString)(((PrtString) String.Format("Still have account balance = {0}, lets try and withdraw more",TMP_tmp19_1)));
                PModule.runtime.Logger.WriteLine("<PrintLog> " + TMP_tmp20_1);
                currentMachine.TryGotoState<WithdrawMoney>();
                return;
            }
        }
        public PrtInt WithdrawAmount()
        {
            Client currentMachine = this;
            PrtInt TMP_tmp0_12 = ((PrtInt)0);
            PrtInt TMP_tmp1_12 = ((PrtInt)0);
            TMP_tmp0_12 = (PrtInt)(((PrtInt)currentMachine.TryRandom(currentBalance_2)));
            TMP_tmp1_12 = (PrtInt)((TMP_tmp0_12) + (((PrtInt)1)));
            return ((PrtInt)((IPrtValue)TMP_tmp1_12)?.Clone());
        }
        public void Anon_8(Event currentMachine_dequeuedEvent)
        {
            Client currentMachine = this;
            PrtBool TMP_tmp0_13 = ((PrtBool)false);
            PrtString TMP_tmp1_13 = ((PrtString)"");
            PrtString TMP_tmp2_12 = ((PrtString)"");
            TMP_tmp0_13 = (PrtBool)((PrtValues.SafeEquals(currentBalance_2,((PrtInt)10))));
            TMP_tmp1_13 = (PrtString)(((PrtString) String.Format("Hmm, I still have money that I can withdraw but I have reached NoMoneyToWithDraw state!")));
            currentMachine.TryAssert(TMP_tmp0_13,"Assertion Failed: " + TMP_tmp1_13);
            TMP_tmp2_12 = (PrtString)(((PrtString) String.Format("No Money to withdraw, waiting for more deposits!")));
            PModule.runtime.Logger.WriteLine("<PrintLog> " + TMP_tmp2_12);
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Init_2))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon_5))]
        class Init_2 : State
        {
        }
        [OnEntry(nameof(Anon_6))]
        [OnEventDoAction(typeof(eWithDrawResp), nameof(Anon_7))]
        class WithdrawMoney : State
        {
        }
        [OnEntry(nameof(Anon_8))]
        class NoMoneyToWithDraw : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class AbstractBankServer : PMachine
    {
        private PrtMap balance_1 = new PrtMap();
        public class ConstructorEvent : PEvent{public ConstructorEvent(PrtMap val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((PrtMap)value); }
        public AbstractBankServer() {
            this.sends.Add(nameof(eReadQuery));
            this.sends.Add(nameof(eReadQueryResp));
            this.sends.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.sends.Add(nameof(eUpdateQuery));
            this.sends.Add(nameof(eWithDrawReq));
            this.sends.Add(nameof(eWithDrawResp));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eReadQuery));
            this.receives.Add(nameof(eReadQueryResp));
            this.receives.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.receives.Add(nameof(eUpdateQuery));
            this.receives.Add(nameof(eWithDrawReq));
            this.receives.Add(nameof(eWithDrawResp));
            this.receives.Add(nameof(PHalt));
        }
        
        public void Anon_9(Event currentMachine_dequeuedEvent)
        {
            AbstractBankServer currentMachine = this;
            PrtMap init_balance = (PrtMap)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            balance_1 = (PrtMap)(((PrtMap)((IPrtValue)init_balance)?.Clone()));
        }
        public void Anon_10(Event currentMachine_dequeuedEvent)
        {
            AbstractBankServer currentMachine = this;
            PrtNamedTuple wReq_1 = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_14 = ((PrtInt)0);
            PrtBool TMP_tmp1_14 = ((PrtBool)false);
            PrtString TMP_tmp2_13 = ((PrtString)"");
            PrtInt TMP_tmp3_12 = ((PrtInt)0);
            PrtInt TMP_tmp4_10 = ((PrtInt)0);
            PrtInt TMP_tmp5_8 = ((PrtInt)0);
            PrtInt TMP_tmp6_6 = ((PrtInt)0);
            PrtBool TMP_tmp7_6 = ((PrtBool)false);
            PrtInt TMP_tmp8_5 = ((PrtInt)0);
            PrtInt TMP_tmp9_3 = ((PrtInt)0);
            PrtInt TMP_tmp10_3 = ((PrtInt)0);
            PrtInt TMP_tmp11_3 = ((PrtInt)0);
            PrtInt TMP_tmp12_3 = ((PrtInt)0);
            PMachineValue TMP_tmp13_3 = null;
            PMachineValue TMP_tmp14_2 = null;
            PEvent TMP_tmp15_2 = null;
            PrtInt TMP_tmp16_2 = ((PrtInt)0);
            PrtInt TMP_tmp17_2 = ((PrtInt)0);
            PrtInt TMP_tmp18_2 = ((PrtInt)0);
            PrtInt TMP_tmp19_2 = ((PrtInt)0);
            PrtInt TMP_tmp20_2 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp21_1 = (new PrtNamedTuple(new string[]{"status","accountId","balance","rId"},((PrtInt)0), ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            PMachineValue TMP_tmp22_1 = null;
            PMachineValue TMP_tmp23_1 = null;
            PEvent TMP_tmp24_1 = null;
            PrtInt TMP_tmp25_1 = ((PrtInt)0);
            PrtInt TMP_tmp26 = ((PrtInt)0);
            PrtInt TMP_tmp27 = ((PrtInt)0);
            PrtInt TMP_tmp28 = ((PrtInt)0);
            PrtInt TMP_tmp29 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp30 = (new PrtNamedTuple(new string[]{"status","accountId","balance","rId"},((PrtInt)0), ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            TMP_tmp0_14 = (PrtInt)(((PrtNamedTuple)wReq_1)["accountId"]);
            TMP_tmp1_14 = (PrtBool)(((PrtBool)(((PrtMap)balance_1).ContainsKey(TMP_tmp0_14))));
            TMP_tmp2_13 = (PrtString)(((PrtString) String.Format("Invalid accountId received in the withdraw request!")));
            currentMachine.TryAssert(TMP_tmp1_14,"Assertion Failed: " + TMP_tmp2_13);
            TMP_tmp3_12 = (PrtInt)(((PrtNamedTuple)wReq_1)["accountId"]);
            TMP_tmp4_10 = (PrtInt)(((PrtMap)balance_1)[TMP_tmp3_12]);
            TMP_tmp5_8 = (PrtInt)(((PrtNamedTuple)wReq_1)["amount"]);
            TMP_tmp6_6 = (PrtInt)((TMP_tmp4_10) - (TMP_tmp5_8));
            TMP_tmp7_6 = (PrtBool)((TMP_tmp6_6) > (((PrtInt)10)));
            if (TMP_tmp7_6)
            {
                TMP_tmp8_5 = (PrtInt)(((PrtNamedTuple)wReq_1)["accountId"]);
                TMP_tmp9_3 = (PrtInt)(((PrtNamedTuple)wReq_1)["accountId"]);
                TMP_tmp10_3 = (PrtInt)(((PrtMap)balance_1)[TMP_tmp9_3]);
                TMP_tmp11_3 = (PrtInt)(((PrtNamedTuple)wReq_1)["amount"]);
                TMP_tmp12_3 = (PrtInt)((TMP_tmp10_3) - (TMP_tmp11_3));
                ((PrtMap)balance_1)[TMP_tmp8_5] = TMP_tmp12_3;
                TMP_tmp13_3 = (PMachineValue)(((PrtNamedTuple)wReq_1)["source"]);
                TMP_tmp14_2 = (PMachineValue)(((PMachineValue)((IPrtValue)TMP_tmp13_3)?.Clone()));
                TMP_tmp15_2 = (PEvent)(new eWithDrawResp((new PrtNamedTuple(new string[]{"status","accountId","balance","rId"},((PrtInt)0), ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)))));
                TMP_tmp16_2 = (PrtInt)((PrtEnum.Get("WITHDRAW_SUCCESS")));
                TMP_tmp17_2 = (PrtInt)(((PrtNamedTuple)wReq_1)["accountId"]);
                TMP_tmp18_2 = (PrtInt)(((PrtNamedTuple)wReq_1)["accountId"]);
                TMP_tmp19_2 = (PrtInt)(((PrtMap)balance_1)[TMP_tmp18_2]);
                TMP_tmp20_2 = (PrtInt)(((PrtNamedTuple)wReq_1)["rId"]);
                TMP_tmp21_1 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"status","accountId","balance","rId"}, TMP_tmp16_2, TMP_tmp17_2, TMP_tmp19_2, TMP_tmp20_2)));
                currentMachine.TrySendEvent(TMP_tmp14_2, (Event)TMP_tmp15_2, TMP_tmp21_1);
            }
            else
            {
                TMP_tmp22_1 = (PMachineValue)(((PrtNamedTuple)wReq_1)["source"]);
                TMP_tmp23_1 = (PMachineValue)(((PMachineValue)((IPrtValue)TMP_tmp22_1)?.Clone()));
                TMP_tmp24_1 = (PEvent)(new eWithDrawResp((new PrtNamedTuple(new string[]{"status","accountId","balance","rId"},((PrtInt)0), ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)))));
                TMP_tmp25_1 = (PrtInt)((PrtEnum.Get("WITHDRAW_ERROR")));
                TMP_tmp26 = (PrtInt)(((PrtNamedTuple)wReq_1)["accountId"]);
                TMP_tmp27 = (PrtInt)(((PrtNamedTuple)wReq_1)["accountId"]);
                TMP_tmp28 = (PrtInt)(((PrtMap)balance_1)[TMP_tmp27]);
                TMP_tmp29 = (PrtInt)(((PrtNamedTuple)wReq_1)["rId"]);
                TMP_tmp30 = (PrtNamedTuple)((new PrtNamedTuple(new string[]{"status","accountId","balance","rId"}, TMP_tmp25_1, TMP_tmp26, TMP_tmp28, TMP_tmp29)));
                currentMachine.TrySendEvent(TMP_tmp23_1, (Event)TMP_tmp24_1, TMP_tmp30);
            }
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(WaitForWithdrawRequests_1))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon_9))]
        [OnEventDoAction(typeof(eWithDrawReq), nameof(Anon_10))]
        class WaitForWithdrawRequests_1 : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class BankBalanceIsAlwaysCorrect : PMonitor
    {
        private PrtMap bankBalance_1 = new PrtMap();
        private PrtMap pendingWithDraws = new PrtMap();
        static BankBalanceIsAlwaysCorrect() {
            observes.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            observes.Add(nameof(eWithDrawReq));
            observes.Add(nameof(eWithDrawResp));
        }
        
        public void Anon_11(Event currentMachine_dequeuedEvent)
        {
            BankBalanceIsAlwaysCorrect currentMachine = this;
            PrtMap balance_2 = (PrtMap)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            bankBalance_1 = (PrtMap)(((PrtMap)((IPrtValue)balance_2)?.Clone()));
        }
        public void Anon_12(Event currentMachine_dequeuedEvent)
        {
            BankBalanceIsAlwaysCorrect currentMachine = this;
            PrtNamedTuple req = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_15 = ((PrtInt)0);
            PrtBool TMP_tmp1_15 = ((PrtBool)false);
            PrtInt TMP_tmp2_14 = ((PrtInt)0);
            PrtSeq TMP_tmp3_13 = new PrtSeq();
            PrtString TMP_tmp4_11 = ((PrtString)"");
            PrtInt TMP_tmp5_9 = ((PrtInt)0);
            TMP_tmp0_15 = (PrtInt)(((PrtNamedTuple)req)["accountId"]);
            TMP_tmp1_15 = (PrtBool)(((PrtBool)(((PrtMap)bankBalance_1).ContainsKey(TMP_tmp0_15))));
            TMP_tmp2_14 = (PrtInt)(((PrtNamedTuple)req)["accountId"]);
            TMP_tmp3_13 = (PrtSeq)((bankBalance_1).CloneKeys());
            TMP_tmp4_11 = (PrtString)(((PrtString) String.Format("Unknown accountId {0} in the withdraw request. Valid accountIds = {1}",TMP_tmp2_14,TMP_tmp3_13)));
            currentMachine.TryAssert(TMP_tmp1_15,"Assertion Failed: " + TMP_tmp4_11);
            TMP_tmp5_9 = (PrtInt)(((PrtNamedTuple)req)["rId"]);
            ((PrtMap)pendingWithDraws)[TMP_tmp5_9] = (PrtNamedTuple)(((PrtNamedTuple)((IPrtValue)req)?.Clone()));
        }
        public void Anon_13(Event currentMachine_dequeuedEvent)
        {
            BankBalanceIsAlwaysCorrect currentMachine = this;
            PrtNamedTuple resp_2 = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_16 = ((PrtInt)0);
            PrtBool TMP_tmp1_16 = ((PrtBool)false);
            PrtInt TMP_tmp2_15 = ((PrtInt)0);
            PrtString TMP_tmp3_14 = ((PrtString)"");
            PrtInt TMP_tmp4_12 = ((PrtInt)0);
            PrtBool TMP_tmp5_10 = ((PrtBool)false);
            PrtInt TMP_tmp6_7 = ((PrtInt)0);
            PrtString TMP_tmp7_7 = ((PrtString)"");
            PrtInt TMP_tmp8_6 = ((PrtInt)0);
            PrtBool TMP_tmp9_4 = ((PrtBool)false);
            PrtString TMP_tmp10_4 = ((PrtString)"");
            PrtInt TMP_tmp11_4 = ((PrtInt)0);
            PrtBool TMP_tmp12_4 = ((PrtBool)false);
            PrtInt TMP_tmp13_4 = ((PrtInt)0);
            PrtInt TMP_tmp14_3 = ((PrtInt)0);
            PrtInt TMP_tmp15_3 = ((PrtInt)0);
            PrtInt TMP_tmp16_3 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp17_3 = (new PrtNamedTuple(new string[]{"source","accountId","amount","rId"},null, ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            PrtInt TMP_tmp18_3 = ((PrtInt)0);
            PrtInt TMP_tmp19_3 = ((PrtInt)0);
            PrtBool TMP_tmp20_3 = ((PrtBool)false);
            PrtInt TMP_tmp21_2 = ((PrtInt)0);
            PrtInt TMP_tmp22_2 = ((PrtInt)0);
            PrtInt TMP_tmp23_2 = ((PrtInt)0);
            PrtInt TMP_tmp24_2 = ((PrtInt)0);
            PrtInt TMP_tmp25_2 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp26_1 = (new PrtNamedTuple(new string[]{"source","accountId","amount","rId"},null, ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            PrtInt TMP_tmp27_1 = ((PrtInt)0);
            PrtInt TMP_tmp28_1 = ((PrtInt)0);
            PrtString TMP_tmp29_1 = ((PrtString)"");
            PrtInt TMP_tmp30_1 = ((PrtInt)0);
            PrtInt TMP_tmp31 = ((PrtInt)0);
            PrtInt TMP_tmp32 = ((PrtInt)0);
            PrtInt TMP_tmp33 = ((PrtInt)0);
            PrtInt TMP_tmp34 = ((PrtInt)0);
            PrtInt TMP_tmp35 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp36 = (new PrtNamedTuple(new string[]{"source","accountId","amount","rId"},null, ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            PrtInt TMP_tmp37 = ((PrtInt)0);
            PrtInt TMP_tmp38 = ((PrtInt)0);
            PrtBool TMP_tmp39 = ((PrtBool)false);
            PrtInt TMP_tmp40 = ((PrtInt)0);
            PrtNamedTuple TMP_tmp41 = (new PrtNamedTuple(new string[]{"source","accountId","amount","rId"},null, ((PrtInt)0), ((PrtInt)0), ((PrtInt)0)));
            PrtInt TMP_tmp42 = ((PrtInt)0);
            PrtInt TMP_tmp43 = ((PrtInt)0);
            PrtInt TMP_tmp44 = ((PrtInt)0);
            PrtString TMP_tmp45 = ((PrtString)"");
            PrtInt TMP_tmp46 = ((PrtInt)0);
            PrtInt TMP_tmp47 = ((PrtInt)0);
            PrtInt TMP_tmp48 = ((PrtInt)0);
            PrtBool TMP_tmp49 = ((PrtBool)false);
            PrtInt TMP_tmp50 = ((PrtInt)0);
            PrtInt TMP_tmp51 = ((PrtInt)0);
            PrtInt TMP_tmp52 = ((PrtInt)0);
            PrtString TMP_tmp53 = ((PrtString)"");
            TMP_tmp0_16 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
            TMP_tmp1_16 = (PrtBool)(((PrtBool)(((PrtMap)bankBalance_1).ContainsKey(TMP_tmp0_16))));
            TMP_tmp2_15 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
            TMP_tmp3_14 = (PrtString)(((PrtString) String.Format("Unknown accountId {0} in the withdraw response!",TMP_tmp2_15)));
            currentMachine.TryAssert(TMP_tmp1_16,"Assertion Failed: " + TMP_tmp3_14);
            TMP_tmp4_12 = (PrtInt)(((PrtNamedTuple)resp_2)["rId"]);
            TMP_tmp5_10 = (PrtBool)(((PrtBool)(((PrtMap)pendingWithDraws).ContainsKey(TMP_tmp4_12))));
            TMP_tmp6_7 = (PrtInt)(((PrtNamedTuple)resp_2)["rId"]);
            TMP_tmp7_7 = (PrtString)(((PrtString) String.Format("Unknown rId {0} in the withdraw response!",TMP_tmp6_7)));
            currentMachine.TryAssert(TMP_tmp5_10,"Assertion Failed: " + TMP_tmp7_7);
            TMP_tmp8_6 = (PrtInt)(((PrtNamedTuple)resp_2)["balance"]);
            TMP_tmp9_4 = (PrtBool)((TMP_tmp8_6) >= (((PrtInt)10)));
            TMP_tmp10_4 = (PrtString)(((PrtString) String.Format("Bank balance in all accounts must always be greater than or equal to 10!!")));
            currentMachine.TryAssert(TMP_tmp9_4,"Assertion Failed: " + TMP_tmp10_4);
            TMP_tmp11_4 = (PrtInt)(((PrtNamedTuple)resp_2)["status"]);
            TMP_tmp12_4 = (PrtBool)((PrtValues.SafeEquals(PrtValues.Box((long) TMP_tmp11_4),PrtValues.Box((long) (PrtEnum.Get("WITHDRAW_SUCCESS"))))));
            if (TMP_tmp12_4)
            {
                TMP_tmp13_4 = (PrtInt)(((PrtNamedTuple)resp_2)["balance"]);
                TMP_tmp14_3 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
                TMP_tmp15_3 = (PrtInt)(((PrtMap)bankBalance_1)[TMP_tmp14_3]);
                TMP_tmp16_3 = (PrtInt)(((PrtNamedTuple)resp_2)["rId"]);
                TMP_tmp17_3 = (PrtNamedTuple)(((PrtMap)pendingWithDraws)[TMP_tmp16_3]);
                TMP_tmp18_3 = (PrtInt)(((PrtNamedTuple)TMP_tmp17_3)["amount"]);
                TMP_tmp19_3 = (PrtInt)((TMP_tmp15_3) - (TMP_tmp18_3));
                TMP_tmp20_3 = (PrtBool)((PrtValues.SafeEquals(TMP_tmp13_4,TMP_tmp19_3)));
                TMP_tmp21_2 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
                TMP_tmp22_2 = (PrtInt)(((PrtNamedTuple)resp_2)["balance"]);
                TMP_tmp23_2 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
                TMP_tmp24_2 = (PrtInt)(((PrtMap)bankBalance_1)[TMP_tmp23_2]);
                TMP_tmp25_2 = (PrtInt)(((PrtNamedTuple)resp_2)["rId"]);
                TMP_tmp26_1 = (PrtNamedTuple)(((PrtMap)pendingWithDraws)[TMP_tmp25_2]);
                TMP_tmp27_1 = (PrtInt)(((PrtNamedTuple)TMP_tmp26_1)["amount"]);
                TMP_tmp28_1 = (PrtInt)((TMP_tmp24_2) - (TMP_tmp27_1));
                TMP_tmp29_1 = (PrtString)(((PrtString) String.Format("Bank balance for the account {0} is {1} and not the expected value {2}, Bank is lying!",TMP_tmp21_2,TMP_tmp22_2,TMP_tmp28_1)));
                currentMachine.TryAssert(TMP_tmp20_3,"Assertion Failed: " + TMP_tmp29_1);
                TMP_tmp30_1 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
                TMP_tmp31 = (PrtInt)(((PrtNamedTuple)resp_2)["balance"]);
                TMP_tmp32 = (PrtInt)(((PrtInt)((IPrtValue)TMP_tmp31)?.Clone()));
                ((PrtMap)bankBalance_1)[TMP_tmp30_1] = TMP_tmp32;
            }
            else
            {
                TMP_tmp33 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
                TMP_tmp34 = (PrtInt)(((PrtMap)bankBalance_1)[TMP_tmp33]);
                TMP_tmp35 = (PrtInt)(((PrtNamedTuple)resp_2)["rId"]);
                TMP_tmp36 = (PrtNamedTuple)(((PrtMap)pendingWithDraws)[TMP_tmp35]);
                TMP_tmp37 = (PrtInt)(((PrtNamedTuple)TMP_tmp36)["amount"]);
                TMP_tmp38 = (PrtInt)((TMP_tmp34) - (TMP_tmp37));
                TMP_tmp39 = (PrtBool)((TMP_tmp38) < (((PrtInt)10)));
                TMP_tmp40 = (PrtInt)(((PrtNamedTuple)resp_2)["rId"]);
                TMP_tmp41 = (PrtNamedTuple)(((PrtMap)pendingWithDraws)[TMP_tmp40]);
                TMP_tmp42 = (PrtInt)(((PrtNamedTuple)TMP_tmp41)["amount"]);
                TMP_tmp43 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
                TMP_tmp44 = (PrtInt)(((PrtMap)bankBalance_1)[TMP_tmp43]);
                TMP_tmp45 = (PrtString)(((PrtString) String.Format("Bank must accept the withdraw request for {0}, bank balance is {1}!",TMP_tmp42,TMP_tmp44)));
                currentMachine.TryAssert(TMP_tmp39,"Assertion Failed: " + TMP_tmp45);
                TMP_tmp46 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
                TMP_tmp47 = (PrtInt)(((PrtMap)bankBalance_1)[TMP_tmp46]);
                TMP_tmp48 = (PrtInt)(((PrtNamedTuple)resp_2)["balance"]);
                TMP_tmp49 = (PrtBool)((PrtValues.SafeEquals(TMP_tmp47,TMP_tmp48)));
                TMP_tmp50 = (PrtInt)(((PrtNamedTuple)resp_2)["accountId"]);
                TMP_tmp51 = (PrtInt)(((PrtMap)bankBalance_1)[TMP_tmp50]);
                TMP_tmp52 = (PrtInt)(((PrtNamedTuple)resp_2)["balance"]);
                TMP_tmp53 = (PrtString)(((PrtString) String.Format("Withdraw failed BUT the account balance changed! actual: {0}, bank said: {1}",TMP_tmp51,TMP_tmp52)));
                currentMachine.TryAssert(TMP_tmp49,"Assertion Failed: " + TMP_tmp53);
            }
        }
        [Start]
        [OnEventGotoState(typeof(eSpec_BankBalanceIsAlwaysCorrect_Init), typeof(WaitForWithDrawReqAndResp), nameof(Anon_11))]
        class Init_3 : State
        {
        }
        [OnEventDoAction(typeof(eWithDrawReq), nameof(Anon_12))]
        [OnEventDoAction(typeof(eWithDrawResp), nameof(Anon_13))]
        class WaitForWithDrawReqAndResp : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class GuaranteedWithDrawProgress : PMonitor
    {
        private PrtSet pendingWDReqs = new PrtSet();
        static GuaranteedWithDrawProgress() {
            observes.Add(nameof(eWithDrawReq));
            observes.Add(nameof(eWithDrawResp));
        }
        
        public void Anon_14(Event currentMachine_dequeuedEvent)
        {
            GuaranteedWithDrawProgress currentMachine = this;
            PrtNamedTuple req_1 = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_17 = ((PrtInt)0);
            TMP_tmp0_17 = (PrtInt)(((PrtNamedTuple)req_1)["rId"]);
            ((PrtSet)pendingWDReqs).Add(TMP_tmp0_17);
        }
        public void Anon_15(Event currentMachine_dequeuedEvent)
        {
            GuaranteedWithDrawProgress currentMachine = this;
            PrtNamedTuple resp_3 = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_18 = ((PrtInt)0);
            PrtBool TMP_tmp1_17 = ((PrtBool)false);
            PrtInt TMP_tmp2_16 = ((PrtInt)0);
            PrtSet TMP_tmp3_15 = new PrtSet();
            PrtString TMP_tmp4_13 = ((PrtString)"");
            PrtInt TMP_tmp5_11 = ((PrtInt)0);
            PrtInt TMP_tmp6_8 = ((PrtInt)0);
            PrtBool TMP_tmp7_8 = ((PrtBool)false);
            TMP_tmp0_18 = (PrtInt)(((PrtNamedTuple)resp_3)["rId"]);
            TMP_tmp1_17 = (PrtBool)(((PrtBool)(((PrtSet)pendingWDReqs).Contains(TMP_tmp0_18))));
            TMP_tmp2_16 = (PrtInt)(((PrtNamedTuple)resp_3)["rId"]);
            TMP_tmp3_15 = (PrtSet)(((PrtSet)((IPrtValue)pendingWDReqs)?.Clone()));
            TMP_tmp4_13 = (PrtString)(((PrtString) String.Format("unexpected rId: {0} received, expected one of {1}",TMP_tmp2_16,TMP_tmp3_15)));
            currentMachine.TryAssert(TMP_tmp1_17,"Assertion Failed: " + TMP_tmp4_13);
            TMP_tmp5_11 = (PrtInt)(((PrtNamedTuple)resp_3)["rId"]);
            ((PrtSet)pendingWDReqs).Remove(TMP_tmp5_11);
            TMP_tmp6_8 = (PrtInt)(((PrtInt)(pendingWDReqs).Count));
            TMP_tmp7_8 = (PrtBool)((PrtValues.SafeEquals(TMP_tmp6_8,((PrtInt)0))));
            if (TMP_tmp7_8)
            {
                currentMachine.TryGotoState<NopendingRequests>();
                return;
            }
        }
        public void Anon_16(Event currentMachine_dequeuedEvent)
        {
            GuaranteedWithDrawProgress currentMachine = this;
            PrtNamedTuple req_2 = (PrtNamedTuple)(gotoPayload ?? ((PEvent)currentMachine_dequeuedEvent).Payload);
            this.gotoPayload = null;
            PrtInt TMP_tmp0_19 = ((PrtInt)0);
            TMP_tmp0_19 = (PrtInt)(((PrtNamedTuple)req_2)["rId"]);
            ((PrtSet)pendingWDReqs).Add(TMP_tmp0_19);
        }
        [Start]
        [OnEventGotoState(typeof(eWithDrawReq), typeof(PendingReqs), nameof(Anon_14))]
        class NopendingRequests : State
        {
        }
        [Hot]
        [OnEventDoAction(typeof(eWithDrawResp), nameof(Anon_15))]
        [OnEventGotoState(typeof(eWithDrawReq), typeof(PendingReqs), nameof(Anon_16))]
        class PendingReqs : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class TestWithSingleClient : PMachine
    {
        public class ConstructorEvent : PEvent{public ConstructorEvent(IPrtValue val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((IPrtValue)value); }
        public TestWithSingleClient() {
            this.sends.Add(nameof(eReadQuery));
            this.sends.Add(nameof(eReadQueryResp));
            this.sends.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.sends.Add(nameof(eUpdateQuery));
            this.sends.Add(nameof(eWithDrawReq));
            this.sends.Add(nameof(eWithDrawResp));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eReadQuery));
            this.receives.Add(nameof(eReadQueryResp));
            this.receives.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.receives.Add(nameof(eUpdateQuery));
            this.receives.Add(nameof(eWithDrawReq));
            this.receives.Add(nameof(eWithDrawResp));
            this.receives.Add(nameof(PHalt));
            this.creates.Add(nameof(I_BankServer));
            this.creates.Add(nameof(I_Client));
        }
        
        public void Anon_17(Event currentMachine_dequeuedEvent)
        {
            TestWithSingleClient currentMachine = this;
            PrtInt TMP_tmp0_20 = ((PrtInt)0);
            TMP_tmp0_20 = (PrtInt)(((PrtInt)1));
            GlobalFunctions.SetupClientServerSystem(TMP_tmp0_20, currentMachine);
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Init_4))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon_17))]
        class Init_4 : State
        {
        }
    }
}
namespace PImplementation
{
    internal partial class TestWithMultipleClients : PMachine
    {
        public class ConstructorEvent : PEvent{public ConstructorEvent(IPrtValue val) : base(val) { }}
        
        protected override Event GetConstructorEvent(IPrtValue value) { return new ConstructorEvent((IPrtValue)value); }
        public TestWithMultipleClients() {
            this.sends.Add(nameof(eReadQuery));
            this.sends.Add(nameof(eReadQueryResp));
            this.sends.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.sends.Add(nameof(eUpdateQuery));
            this.sends.Add(nameof(eWithDrawReq));
            this.sends.Add(nameof(eWithDrawResp));
            this.sends.Add(nameof(PHalt));
            this.receives.Add(nameof(eReadQuery));
            this.receives.Add(nameof(eReadQueryResp));
            this.receives.Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            this.receives.Add(nameof(eUpdateQuery));
            this.receives.Add(nameof(eWithDrawReq));
            this.receives.Add(nameof(eWithDrawResp));
            this.receives.Add(nameof(PHalt));
            this.creates.Add(nameof(I_BankServer));
            this.creates.Add(nameof(I_Client));
        }
        
        public void Anon_18(Event currentMachine_dequeuedEvent)
        {
            TestWithMultipleClients currentMachine = this;
            PrtInt TMP_tmp0_21 = ((PrtInt)0);
            PrtInt TMP_tmp1_18 = ((PrtInt)0);
            TMP_tmp0_21 = (PrtInt)(((PrtInt)currentMachine.TryRandom(((PrtInt)3))));
            TMP_tmp1_18 = (PrtInt)((TMP_tmp0_21) + (((PrtInt)2)));
            GlobalFunctions.SetupClientServerSystem(TMP_tmp1_18, currentMachine);
        }
        [Start]
        [OnEntry(nameof(InitializeParametersFunction))]
        [OnEventGotoState(typeof(ConstructorEvent), typeof(Init_5))]
        class __InitState__ : State { }
        
        [OnEntry(nameof(Anon_18))]
        class Init_5 : State
        {
        }
    }
}
namespace PImplementation
{
    public class tcSingleClient {
        public static void InitializeLinkMap() {
            PModule.linkMap.Clear();
            PModule.linkMap[nameof(I_Client)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_BankServer)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_BankServer)].Add(nameof(I_Database), nameof(I_Database));
            PModule.linkMap[nameof(I_Database)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_TestWithSingleClient)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_TestWithSingleClient)].Add(nameof(I_BankServer), nameof(I_BankServer));
            PModule.linkMap[nameof(I_TestWithSingleClient)].Add(nameof(I_Client), nameof(I_Client));
        }
        
        public static void InitializeInterfaceDefMap() {
            PModule.interfaceDefinitionMap.Clear();
            PModule.interfaceDefinitionMap.Add(nameof(I_Client), typeof(Client));
            PModule.interfaceDefinitionMap.Add(nameof(I_BankServer), typeof(BankServer));
            PModule.interfaceDefinitionMap.Add(nameof(I_Database), typeof(Database));
            PModule.interfaceDefinitionMap.Add(nameof(I_TestWithSingleClient), typeof(TestWithSingleClient));
        }
        
        public static void InitializeMonitorObserves() {
            PModule.monitorObserves.Clear();
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)] = new List<string>();
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)].Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)].Add(nameof(eWithDrawReq));
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)].Add(nameof(eWithDrawResp));
            PModule.monitorObserves[nameof(GuaranteedWithDrawProgress)] = new List<string>();
            PModule.monitorObserves[nameof(GuaranteedWithDrawProgress)].Add(nameof(eWithDrawReq));
            PModule.monitorObserves[nameof(GuaranteedWithDrawProgress)].Add(nameof(eWithDrawResp));
        }
        
        public static void InitializeMonitorMap(IActorRuntime runtime) {
            PModule.monitorMap.Clear();
            PModule.monitorMap[nameof(I_Client)] = new List<Type>();
            PModule.monitorMap[nameof(I_Client)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_Client)].Add(typeof(GuaranteedWithDrawProgress));
            PModule.monitorMap[nameof(I_BankServer)] = new List<Type>();
            PModule.monitorMap[nameof(I_BankServer)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_BankServer)].Add(typeof(GuaranteedWithDrawProgress));
            PModule.monitorMap[nameof(I_Database)] = new List<Type>();
            PModule.monitorMap[nameof(I_Database)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_Database)].Add(typeof(GuaranteedWithDrawProgress));
            PModule.monitorMap[nameof(I_TestWithSingleClient)] = new List<Type>();
            PModule.monitorMap[nameof(I_TestWithSingleClient)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_TestWithSingleClient)].Add(typeof(GuaranteedWithDrawProgress));
            runtime.RegisterMonitor<BankBalanceIsAlwaysCorrect>();
            runtime.RegisterMonitor<GuaranteedWithDrawProgress>();
        }
        
        
        [Microsoft.Coyote.SystematicTesting.Test]
        public static void Execute(IActorRuntime runtime) {
            runtime.RegisterLog(new PLogFormatter());
            PModule.runtime = runtime;
            PHelper.InitializeInterfaces();
            PHelper.InitializeEnums();
            InitializeLinkMap();
            InitializeInterfaceDefMap();
            InitializeMonitorMap(runtime);
            InitializeMonitorObserves();
            runtime.CreateActor(typeof(_GodMachine), new _GodMachine.Config(typeof(TestWithSingleClient)));
        }
    }
}
namespace PImplementation
{
    public class tcMultipleClients {
        public static void InitializeLinkMap() {
            PModule.linkMap.Clear();
            PModule.linkMap[nameof(I_Client)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_BankServer)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_BankServer)].Add(nameof(I_Database), nameof(I_Database));
            PModule.linkMap[nameof(I_Database)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_TestWithMultipleClients)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_TestWithMultipleClients)].Add(nameof(I_BankServer), nameof(I_BankServer));
            PModule.linkMap[nameof(I_TestWithMultipleClients)].Add(nameof(I_Client), nameof(I_Client));
        }
        
        public static void InitializeInterfaceDefMap() {
            PModule.interfaceDefinitionMap.Clear();
            PModule.interfaceDefinitionMap.Add(nameof(I_Client), typeof(Client));
            PModule.interfaceDefinitionMap.Add(nameof(I_BankServer), typeof(BankServer));
            PModule.interfaceDefinitionMap.Add(nameof(I_Database), typeof(Database));
            PModule.interfaceDefinitionMap.Add(nameof(I_TestWithMultipleClients), typeof(TestWithMultipleClients));
        }
        
        public static void InitializeMonitorObserves() {
            PModule.monitorObserves.Clear();
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)] = new List<string>();
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)].Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)].Add(nameof(eWithDrawReq));
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)].Add(nameof(eWithDrawResp));
            PModule.monitorObserves[nameof(GuaranteedWithDrawProgress)] = new List<string>();
            PModule.monitorObserves[nameof(GuaranteedWithDrawProgress)].Add(nameof(eWithDrawReq));
            PModule.monitorObserves[nameof(GuaranteedWithDrawProgress)].Add(nameof(eWithDrawResp));
        }
        
        public static void InitializeMonitorMap(IActorRuntime runtime) {
            PModule.monitorMap.Clear();
            PModule.monitorMap[nameof(I_Client)] = new List<Type>();
            PModule.monitorMap[nameof(I_Client)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_Client)].Add(typeof(GuaranteedWithDrawProgress));
            PModule.monitorMap[nameof(I_BankServer)] = new List<Type>();
            PModule.monitorMap[nameof(I_BankServer)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_BankServer)].Add(typeof(GuaranteedWithDrawProgress));
            PModule.monitorMap[nameof(I_Database)] = new List<Type>();
            PModule.monitorMap[nameof(I_Database)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_Database)].Add(typeof(GuaranteedWithDrawProgress));
            PModule.monitorMap[nameof(I_TestWithMultipleClients)] = new List<Type>();
            PModule.monitorMap[nameof(I_TestWithMultipleClients)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_TestWithMultipleClients)].Add(typeof(GuaranteedWithDrawProgress));
            runtime.RegisterMonitor<BankBalanceIsAlwaysCorrect>();
            runtime.RegisterMonitor<GuaranteedWithDrawProgress>();
        }
        
        
        [Microsoft.Coyote.SystematicTesting.Test]
        public static void Execute(IActorRuntime runtime) {
            runtime.RegisterLog(new PLogFormatter());
            PModule.runtime = runtime;
            PHelper.InitializeInterfaces();
            PHelper.InitializeEnums();
            InitializeLinkMap();
            InitializeInterfaceDefMap();
            InitializeMonitorMap(runtime);
            InitializeMonitorObserves();
            runtime.CreateActor(typeof(_GodMachine), new _GodMachine.Config(typeof(TestWithMultipleClients)));
        }
    }
}
namespace PImplementation
{
    public class tcSingleClientAbstractServer {
        public static void InitializeLinkMap() {
            PModule.linkMap.Clear();
            PModule.linkMap[nameof(I_Client)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_BankServer)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_TestWithSingleClient)] = new Dictionary<string, string>();
            PModule.linkMap[nameof(I_TestWithSingleClient)].Add(nameof(I_BankServer), nameof(I_BankServer));
            PModule.linkMap[nameof(I_TestWithSingleClient)].Add(nameof(I_Client), nameof(I_Client));
        }
        
        public static void InitializeInterfaceDefMap() {
            PModule.interfaceDefinitionMap.Clear();
            PModule.interfaceDefinitionMap.Add(nameof(I_Client), typeof(Client));
            PModule.interfaceDefinitionMap.Add(nameof(I_BankServer), typeof(AbstractBankServer));
            PModule.interfaceDefinitionMap.Add(nameof(I_TestWithSingleClient), typeof(TestWithSingleClient));
        }
        
        public static void InitializeMonitorObserves() {
            PModule.monitorObserves.Clear();
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)] = new List<string>();
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)].Add(nameof(eSpec_BankBalanceIsAlwaysCorrect_Init));
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)].Add(nameof(eWithDrawReq));
            PModule.monitorObserves[nameof(BankBalanceIsAlwaysCorrect)].Add(nameof(eWithDrawResp));
            PModule.monitorObserves[nameof(GuaranteedWithDrawProgress)] = new List<string>();
            PModule.monitorObserves[nameof(GuaranteedWithDrawProgress)].Add(nameof(eWithDrawReq));
            PModule.monitorObserves[nameof(GuaranteedWithDrawProgress)].Add(nameof(eWithDrawResp));
        }
        
        public static void InitializeMonitorMap(IActorRuntime runtime) {
            PModule.monitorMap.Clear();
            PModule.monitorMap[nameof(I_Client)] = new List<Type>();
            PModule.monitorMap[nameof(I_Client)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_Client)].Add(typeof(GuaranteedWithDrawProgress));
            PModule.monitorMap[nameof(I_BankServer)] = new List<Type>();
            PModule.monitorMap[nameof(I_BankServer)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_BankServer)].Add(typeof(GuaranteedWithDrawProgress));
            PModule.monitorMap[nameof(I_TestWithSingleClient)] = new List<Type>();
            PModule.monitorMap[nameof(I_TestWithSingleClient)].Add(typeof(BankBalanceIsAlwaysCorrect));
            PModule.monitorMap[nameof(I_TestWithSingleClient)].Add(typeof(GuaranteedWithDrawProgress));
            runtime.RegisterMonitor<BankBalanceIsAlwaysCorrect>();
            runtime.RegisterMonitor<GuaranteedWithDrawProgress>();
        }
        
        
        [Microsoft.Coyote.SystematicTesting.Test]
        public static void Execute(IActorRuntime runtime) {
            runtime.RegisterLog(new PLogFormatter());
            PModule.runtime = runtime;
            PHelper.InitializeInterfaces();
            PHelper.InitializeEnums();
            InitializeLinkMap();
            InitializeInterfaceDefMap();
            InitializeMonitorMap(runtime);
            InitializeMonitorObserves();
            runtime.CreateActor(typeof(_GodMachine), new _GodMachine.Config(typeof(TestWithSingleClient)));
        }
    }
}
// TODO: NamedModule Client_1
// TODO: NamedModule Bank
// TODO: NamedModule AbstractBank
namespace PImplementation
{
    public class I_BankServer : PMachineValue {
        public I_BankServer (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_Database : PMachineValue {
        public I_Database (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_Client : PMachineValue {
        public I_Client (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_AbstractBankServer : PMachineValue {
        public I_AbstractBankServer (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_TestWithSingleClient : PMachineValue {
        public I_TestWithSingleClient (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public class I_TestWithMultipleClients : PMachineValue {
        public I_TestWithMultipleClients (ActorId machine, List<string> permissions) : base(machine, permissions) { }
    }
    
    public partial class PHelper {
        public static void InitializeInterfaces() {
            PInterfaces.Clear();
            PInterfaces.AddInterface(nameof(I_BankServer), nameof(eReadQuery), nameof(eReadQueryResp), nameof(eSpec_BankBalanceIsAlwaysCorrect_Init), nameof(eUpdateQuery), nameof(eWithDrawReq), nameof(eWithDrawResp), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_Database), nameof(eReadQuery), nameof(eReadQueryResp), nameof(eSpec_BankBalanceIsAlwaysCorrect_Init), nameof(eUpdateQuery), nameof(eWithDrawReq), nameof(eWithDrawResp), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_Client), nameof(eReadQuery), nameof(eReadQueryResp), nameof(eSpec_BankBalanceIsAlwaysCorrect_Init), nameof(eUpdateQuery), nameof(eWithDrawReq), nameof(eWithDrawResp), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_AbstractBankServer), nameof(eReadQuery), nameof(eReadQueryResp), nameof(eSpec_BankBalanceIsAlwaysCorrect_Init), nameof(eUpdateQuery), nameof(eWithDrawReq), nameof(eWithDrawResp), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_TestWithSingleClient), nameof(eReadQuery), nameof(eReadQueryResp), nameof(eSpec_BankBalanceIsAlwaysCorrect_Init), nameof(eUpdateQuery), nameof(eWithDrawReq), nameof(eWithDrawResp), nameof(PHalt));
            PInterfaces.AddInterface(nameof(I_TestWithMultipleClients), nameof(eReadQuery), nameof(eReadQueryResp), nameof(eSpec_BankBalanceIsAlwaysCorrect_Init), nameof(eUpdateQuery), nameof(eWithDrawReq), nameof(eWithDrawResp), nameof(PHalt));
        }
    }
    
}
namespace PImplementation
{
    public partial class PHelper {
        public static void InitializeEnums() {
            PrtEnum.Clear();
            PrtEnum.AddEnumElements(new [] {"WITHDRAW_SUCCESS","WITHDRAW_ERROR"}, new [] {0,1});
        }
    }
    
}
#pragma warning restore 162, 219, 414
