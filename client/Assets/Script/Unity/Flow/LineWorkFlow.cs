namespace SGF.Unity.Flow {

    using System.Collections.Generic;

    //PS:目前只支持单线单任务的任务流。
    //TODO:增加并行的任务流

    public enum FlowState {
        DEFAULT = 0,
        EXCTEUS = 1, //正在运行中
        PAUSE = 2,
        RESUME = 3,
        STOP = 9998,
        COMPLATE = 9999, //必须最大 
    }
    /// <summary>
    /// 内部状态
    /// </summary>
    public enum FlowInternalState {
        //这个事件的改变不会向上外传递事件
        INITED = 0, //必须为最小
        ALIVED = 1,
        DEALIVED = 2,
        PAUSEED = 3,
    }

    public delegate void OnFlowPercentChangedAction (IFlow flow);
    public delegate void OnFlowStateChangeAction (IFlow flow);
    public delegate void OnFlowError (IFlow sender);
    public delegate void OnFlowSwitchAction (IFlow old, IFlow newFlow);
    public delegate void OnFlowGropCompate ();
    public delegate void OnFlowGropError (IFlow flow);

    /// <summary>
    /// Flow 节点
    /// </summary>
    public interface IFlow {
        /// <summary> 启动任务 </summary>
        void StartAsy ();

        /// <summary> 停止任务  </summary>
        void Stop ();

        /// <summary> 暂停任务 </summary>
        void Pause ();

        /// <summary> 再次执行任务 </summary>
        void Resum ();

        /// <summary> 当前任务节点的状态 </summary>
        /// <value>当前任务流的工作状态</value>
        FlowState State { get; }

        /// <summary> 当前进行的任务进度 </summary>
        /// <value>0~1</value>
        float Percent { get; }

        /// <summary>
        /// 当前flow的索引
        /// </summary>
        /// <value></value>
        int Index { get; set; }

        /// <summary> 任务节点状态发生变化 </summary>
        event OnFlowStateChangeAction OnFlowStateChangedEvent;

        /// <summary> 任务执行期发生了错误的事件 </summary>
        event OnFlowError OnFlowErrorChangedEvent;

        /// <summary> 当进行中的任务进度发生变化 </summary>
        event OnFlowPercentChangedAction OnFlowPercentChangedEvent;

    }

    public abstract class BaseFlow : IFlow {

        public int Index { get; set; }

        private FlowInternalState internalState = FlowInternalState.INITED;

        protected FlowInternalState InternalState {
            get { return internalState; }
            set { this.internalState = value; }
        }

        private FlowState state;
        public FlowState State {
            get { return this.state; }
            private set {
                var oldeValue = this.state;
                var newValue = value;
                this.state = value;
                if (oldeValue != newValue) {
                    this.OnStateChangedProcess (oldeValue, newValue);
                }

            }
        }

        private float percent = 0.0f;
        public float Percent {
            get { return this.percent; } set {
                if (System.Math.Abs (this.percent - value) > 0.0001) {
                    this.percent = value;
                    if (this.OnFlowPercentChangedEvent != null) {
                        this.OnFlowPercentChangedEvent.Invoke (this);
                    }
                }
            }
        }

        public event OnFlowStateChangeAction OnFlowStateChangedEvent;

        public event OnFlowError OnFlowErrorChangedEvent;

        public event OnFlowPercentChangedAction OnFlowPercentChangedEvent;

        protected BaseFlow () {
            this.OnInit ();
        }

        public virtual void Pause () {
            this.State = FlowState.PAUSE;
            this.internalState = FlowInternalState.PAUSEED;
        }

        public virtual void StartAsy () {
            this.State = FlowState.EXCTEUS;
            this.internalState = FlowInternalState.ALIVED;
        }

        public virtual void Stop () {
            this.State = FlowState.STOP;
            this.internalState = FlowInternalState.DEALIVED;
        }

        private void OnStateChangedProcess (FlowState oldValue, FlowState newValue) {
            switch (newValue) {
                case FlowState.EXCTEUS:
                case FlowState.PAUSE:
                case FlowState.STOP:
                    if (this.OnFlowStateChangedEvent != null)
                        this.OnFlowStateChangedEvent (this);
                    this.OnUnInit ();
                    break;
                case FlowState.COMPLATE:
                    if (this.OnFlowPercentChangedEvent != null)
                        this.OnFlowPercentChangedEvent (this);
                    if (this.OnFlowStateChangedEvent != null)
                        this.OnFlowStateChangedEvent (this);
                    this.OnUnInit ();
                    break;
                default:
                    throw new System.Exception ("无法支持的工作流状态！");
            }
        }

        protected virtual void RefPercent (float percent) {
            this.Percent = percent;
        }

        virtual protected void OnInit () {

        }

        virtual protected void OnUnInit () {

        }

        public void Resum () {
            this.State = FlowState.RESUME;
            this.internalState = FlowInternalState.ALIVED;
        }

        protected void SetComplate () {
            this.State = FlowState.COMPLATE;
        }
    }

    public class FlowGroup {

        private List<IFlow> flows;

        public event OnFlowStateChangeAction OnFlowStateChangedEvent;

        public event OnFlowSwitchAction OnFlowSwitchEvent;

        public event OnFlowGropCompate OnFlowGropCompateEvent;

        public event OnFlowGropError OnFlowGropErrorEvent;

        public event OnFlowPercentChangedAction OnFlowPercentChangedEvent;

        private int flowIndex = 0;

        public FlowGroup () {
            this.flows = new List<IFlow> ();
        }

        public void Start () {
            if (flows.Count == 0) return;
            var flow = flows[flowIndex];
            flow.StartAsy ();
        }

        public void StartAsy () {
            if (flows.Count == 0) return;
            var flow = flows[flowIndex];
            foreach (var item in flows) {
                this.RegisterEvents (item);
            }
            flow.StartAsy ();
        }

        public void Stop () {
            var flow = flows[flowIndex];
            flow.Stop ();
            foreach (var item in flows) {
                this.UnRegisterEvents (item);
            }
        }

        public void Pause () {
            flows[flowIndex].Pause ();
        }

        public void InitFlow (params IFlow[] flows) {
            if (flows != null) {
                int index = 0;
                foreach (var item in flows) {
                    item.Index = index;
                    this.flows.Add (item);
                    index++;
                }
            }
        }

        public void AddFlow (IFlow flows) {
            this.flows.Add (flows);
        }

        /// <summary>
        /// 当前正在任务中的任务
        /// </summary>
        /// <returns></returns>
        protected List<IFlow> GetCurrentFlows () {
            return this.flows.FindAll (x => x.State >= FlowState.EXCTEUS && x.State <= FlowState.STOP);
        }

        private void RegisterEvents (IFlow flow) {
            flow.OnFlowStateChangedEvent += this.FlowStateChangedEventTrigger;
            flow.OnFlowErrorChangedEvent += this.FlowErrorEventTrigger;
            flow.OnFlowPercentChangedEvent += this.OnFlowPercentChangedEvent;
        }

        private void UnRegisterEvents (IFlow flow) {
            flow.OnFlowStateChangedEvent -= this.FlowStateChangedEventTrigger;
            flow.OnFlowErrorChangedEvent -= this.FlowErrorEventTrigger;
            flow.OnFlowPercentChangedEvent -= this.OnFlowPercentChangedEvent;
        }

        private void FlowErrorEventTrigger (IFlow flow) {
            if (this.OnFlowPercentChangedEvent != null) {
                this.OnFlowPercentChangedEvent.Invoke (flow);
            }
        }

        private void OnFlowPercentChangedTrigger (IFlow flow) {
            if (this.OnFlowGropErrorEvent != null) {
                this.OnFlowGropErrorEvent.Invoke (flow);
                this.UnRegisterEvents (flow);
            }
        }

        private void FlowStateChangedEventTrigger (IFlow flow) {
            if (flow.State == FlowState.COMPLATE) {
                // Logger.PrintWarring ("回调的flow:" + flow.Index + ":" + flow.State.ToString ());
                this.SwitchFlow ();
            }

            if (OnFlowStateChangedEvent != null) {
                OnFlowStateChangedEvent.Invoke (flow);
            }
        }

        private void SwitchFlow () {
            var currentFlow = flows[this.flowIndex];
            this.flowIndex++; //switch index.
            if (this.flowIndex == this.flows.Count) {
                if (this.OnFlowGropCompateEvent != null) {
                    this.OnFlowGropCompateEvent.Invoke ();
                }
            } else {
                // Logger.PrintWarring ("swithc to new Flow:" + this.flowIndex);
                this.UnRegisterEvents (currentFlow);
                flows[flowIndex].StartAsy ();
            }
        }
    }
}