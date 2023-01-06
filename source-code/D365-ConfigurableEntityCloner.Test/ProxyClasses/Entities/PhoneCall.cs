namespace ConfigurableEntityCloner.Test.ProxyClasses
{

	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("phonecall")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.82")]
	public partial class PhoneCall : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public PhoneCall() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "phonecall";
		
		public const string EntityLogicalCollectionName = "phonecalls";
		
		public const string EntitySetName = "phonecalls";
		
		public const int EntityTypeCode = 4210;
		
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		
		public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
		
		private void OnPropertyChanged(string propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void OnPropertyChanging(string propertyName)
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("activityadditionalparams")]
		public string ActivityAdditionalParams
		{
			get
			{
				return this.GetAttributeValue<string>("activityadditionalparams");
			}
			set
			{
				this.OnPropertyChanging("ActivityAdditionalParams");
				this.SetAttributeValue("activityadditionalparams", value);
				this.OnPropertyChanged("ActivityAdditionalParams");
			}
		}
		
		/// <summary>
		/// Unique identifier of the phone call activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("activityid")]
		public System.Nullable<System.Guid> ActivityId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("activityid");
			}
			set
			{
				this.OnPropertyChanging("ActivityId");
				this.SetAttributeValue("activityid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("ActivityId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("activityid")]
		public override System.Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.ActivityId = value;
			}
		}
		
		/// <summary>
		/// Shows the type of activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("activitytypecode")]
		public string ActivityTypeCode
		{
			get
			{
				return this.GetAttributeValue<string>("activitytypecode");
			}
		}
		
		/// <summary>
		/// Type the number of minutes spent on the phone call. The duration is used in reporting.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("actualdurationminutes")]
		public System.Nullable<int> ActualDurationMinutes
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("actualdurationminutes");
			}
			set
			{
				this.OnPropertyChanging("ActualDurationMinutes");
				this.SetAttributeValue("actualdurationminutes", value);
				this.OnPropertyChanged("ActualDurationMinutes");
			}
		}
		
		/// <summary>
		/// Enter the actual end date and time of the phone call. By default, it displays the date and time when the activity was completed or canceled, but can be edited to capture the actual duration of the phone call.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("actualend")]
		public System.Nullable<System.DateTime> ActualEnd
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("actualend");
			}
			set
			{
				this.OnPropertyChanging("ActualEnd");
				this.SetAttributeValue("actualend", value);
				this.OnPropertyChanged("ActualEnd");
			}
		}
		
		/// <summary>
		/// Enter the actual start date and time for the phone call. By default, it displays the date and time when the activity was created, but can be edited to capture the actual duration of the phone call.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("actualstart")]
		public System.Nullable<System.DateTime> ActualStart
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("actualstart");
			}
			set
			{
				this.OnPropertyChanging("ActualStart");
				this.SetAttributeValue("actualstart", value);
				this.OnPropertyChanged("ActualStart");
			}
		}
		
		/// <summary>
		/// Type a category to identify the phone call type, such as lead gathering or customer follow-up, to tie the phone call to a business group or function.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("category")]
		public string Category
		{
			get
			{
				return this.GetAttributeValue<string>("category");
			}
			set
			{
				this.OnPropertyChanging("Category");
				this.SetAttributeValue("category", value);
				this.OnPropertyChanged("Category");
			}
		}
		
		/// <summary>
		/// Shows who created the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
			}
		}
		
		/// <summary>
		/// Shows the date and time when the record was created. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
		public System.Nullable<System.DateTime> CreatedOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
			}
		}
		
		/// <summary>
		/// Shows who created the record on behalf of another user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
			}
		}
		
		/// <summary>
		/// Type additional information to describe the phone call, such as the primary message or the products and services discussed.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
		public string Description
		{
			get
			{
				return this.GetAttributeValue<string>("description");
			}
			set
			{
				this.OnPropertyChanging("Description");
				this.SetAttributeValue("description", value);
				this.OnPropertyChanged("Description");
			}
		}
		
		/// <summary>
		/// Select the direction of the phone call as incoming or outbound.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("directioncode")]
		public System.Nullable<bool> DirectionCode
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("directioncode");
			}
			set
			{
				this.OnPropertyChanging("DirectionCode");
				this.SetAttributeValue("directioncode", value);
				this.OnPropertyChanged("DirectionCode");
			}
		}
		
		/// <summary>
		/// Shows the conversion rate of the record's currency. The exchange rate is used to convert all money fields in the record from the local currency to the system's default currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("exchangerate")]
		public System.Nullable<decimal> ExchangeRate
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("exchangerate");
			}
		}
		
		/// <summary>
		/// Enter the account, contact, lead, or user who made the phone call.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("from")]
		public System.Collections.Generic.IEnumerable<Microsoft.Xrm.Sdk.Entity> From
		{
			get
			{
				Microsoft.Xrm.Sdk.EntityCollection collection = this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityCollection>("from");
				if (((collection != null) 
							&& (collection.Entities != null)))
				{
					return System.Linq.Enumerable.Cast<Microsoft.Xrm.Sdk.Entity>(collection.Entities);
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.OnPropertyChanging("From");
				if ((value == null))
				{
					this.SetAttributeValue("from", value);
				}
				else
				{
					this.SetAttributeValue("from", new Microsoft.Xrm.Sdk.EntityCollection(new System.Collections.Generic.List<Microsoft.Xrm.Sdk.Entity>(value)));
				}
				this.OnPropertyChanged("From");
			}
		}
		
		/// <summary>
		/// Unique identifier of the data import or data migration that created this record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("importsequencenumber")]
		public System.Nullable<int> ImportSequenceNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("importsequencenumber");
			}
			set
			{
				this.OnPropertyChanging("ImportSequenceNumber");
				this.SetAttributeValue("importsequencenumber", value);
				this.OnPropertyChanged("ImportSequenceNumber");
			}
		}
		
		/// <summary>
		/// Information which specifies whether the phone call activity was billed as part of resolving a case.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isbilled")]
		public System.Nullable<bool> IsBilled
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isbilled");
			}
			set
			{
				this.OnPropertyChanging("IsBilled");
				this.SetAttributeValue("isbilled", value);
				this.OnPropertyChanged("IsBilled");
			}
		}
		
		/// <summary>
		/// Information regarding whether the activity is a regular activity type or event type.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isregularactivity")]
		public System.Nullable<bool> IsRegularActivity
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isregularactivity");
			}
		}
		
		/// <summary>
		/// Indication which specifies if the phone call activity was created by a workflow rule.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isworkflowcreated")]
		public System.Nullable<bool> IsWorkflowCreated
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isworkflowcreated");
			}
			set
			{
				this.OnPropertyChanging("IsWorkflowCreated");
				this.SetAttributeValue("isworkflowcreated", value);
				this.OnPropertyChanged("IsWorkflowCreated");
			}
		}
		
		/// <summary>
		/// Contains the date and time stamp of the last on hold time.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("lastonholdtime")]
		public System.Nullable<System.DateTime> LastOnHoldTime
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("lastonholdtime");
			}
			set
			{
				this.OnPropertyChanging("LastOnHoldTime");
				this.SetAttributeValue("lastonholdtime", value);
				this.OnPropertyChanged("LastOnHoldTime");
			}
		}
		
		/// <summary>
		/// Select whether a voice mail was left for the person.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("leftvoicemail")]
		public System.Nullable<bool> LeftVoiceMail
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("leftvoicemail");
			}
			set
			{
				this.OnPropertyChanging("LeftVoiceMail");
				this.SetAttributeValue("leftvoicemail", value);
				this.OnPropertyChanged("LeftVoiceMail");
			}
		}
		
		/// <summary>
		/// Shows who last updated the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
			}
		}
		
		/// <summary>
		/// Shows the date and time when the record was last updated. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
		public System.Nullable<System.DateTime> ModifiedOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
			}
		}
		
		/// <summary>
		/// Shows who last updated the record on behalf of another user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// Shows how long, in minutes, that the record was on hold.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("onholdtime")]
		public System.Nullable<int> OnHoldTime
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("onholdtime");
			}
		}
		
		/// <summary>
		/// Date and time that the record was migrated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overriddencreatedon")]
		public System.Nullable<System.DateTime> OverriddenCreatedOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("overriddencreatedon");
			}
			set
			{
				this.OnPropertyChanging("OverriddenCreatedOn");
				this.SetAttributeValue("overriddencreatedon", value);
				this.OnPropertyChanged("OverriddenCreatedOn");
			}
		}
		
		/// <summary>
		/// Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ownerid")]
		public Microsoft.Xrm.Sdk.EntityReference OwnerId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("ownerid");
			}
			set
			{
				this.OnPropertyChanging("OwnerId");
				this.SetAttributeValue("ownerid", value);
				this.OnPropertyChanged("OwnerId");
			}
		}
		
		/// <summary>
		/// Unique identifier of the business unit that owns the phone call activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningbusinessunit")]
		public Microsoft.Xrm.Sdk.EntityReference OwningBusinessUnit
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningbusinessunit");
			}
		}
		
		/// <summary>
		/// Unique identifier of the team that owns the phone call activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		public Microsoft.Xrm.Sdk.EntityReference OwningTeam
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningteam");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user that owns the phone call activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		public Microsoft.Xrm.Sdk.EntityReference OwningUser
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owninguser");
			}
		}
		
		/// <summary>
		/// Type the phone number.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("phonenumber")]
		public string PhoneNumber
		{
			get
			{
				return this.GetAttributeValue<string>("phonenumber");
			}
			set
			{
				this.OnPropertyChanging("PhoneNumber");
				this.SetAttributeValue("phonenumber", value);
				this.OnPropertyChanged("PhoneNumber");
			}
		}
		
		/// <summary>
		/// Select the priority so that preferred customers or critical issues are handled quickly.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("prioritycode")]
		public System.Nullable<ConfigurableEntityCloner.Test.ProxyClasses.phonecall_prioritycode> PriorityCode
		{
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("prioritycode");
				if ((optionSet != null))
				{
					return ((ConfigurableEntityCloner.Test.ProxyClasses.phonecall_prioritycode)(System.Enum.ToObject(typeof(ConfigurableEntityCloner.Test.ProxyClasses.phonecall_prioritycode), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.OnPropertyChanging("PriorityCode");
				if ((value == null))
				{
					this.SetAttributeValue("prioritycode", null);
				}
				else
				{
					this.SetAttributeValue("prioritycode", new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
				}
				this.OnPropertyChanged("PriorityCode");
			}
		}
		
		/// <summary>
		/// Shows the ID of the process.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("processid")]
		public System.Nullable<System.Guid> ProcessId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("processid");
			}
			set
			{
				this.OnPropertyChanging("ProcessId");
				this.SetAttributeValue("processid", value);
				this.OnPropertyChanged("ProcessId");
			}
		}
		
		/// <summary>
		/// Choose the record that the phone call relates to.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("regardingobjectid")]
		public Microsoft.Xrm.Sdk.EntityReference RegardingObjectId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("regardingobjectid");
			}
			set
			{
				this.OnPropertyChanging("RegardingObjectId");
				this.SetAttributeValue("regardingobjectid", value);
				this.OnPropertyChanged("RegardingObjectId");
			}
		}
		
		/// <summary>
		/// Scheduled duration of the phone call activity, specified in minutes.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("scheduleddurationminutes")]
		public System.Nullable<int> ScheduledDurationMinutes
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("scheduleddurationminutes");
			}
		}
		
		/// <summary>
		/// Enter the expected due date and time.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("scheduledend")]
		public System.Nullable<System.DateTime> ScheduledEnd
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("scheduledend");
			}
			set
			{
				this.OnPropertyChanging("ScheduledEnd");
				this.SetAttributeValue("scheduledend", value);
				this.OnPropertyChanged("ScheduledEnd");
			}
		}
		
		/// <summary>
		/// Enter the expected due date and time.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("scheduledstart")]
		public System.Nullable<System.DateTime> ScheduledStart
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("scheduledstart");
			}
			set
			{
				this.OnPropertyChanging("ScheduledStart");
				this.SetAttributeValue("scheduledstart", value);
				this.OnPropertyChanged("ScheduledStart");
			}
		}
		
		/// <summary>
		/// Choose the service level agreement (SLA) that you want to apply to the Phone Call record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("slaid")]
		public Microsoft.Xrm.Sdk.EntityReference SLAId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("slaid");
			}
			set
			{
				this.OnPropertyChanging("SLAId");
				this.SetAttributeValue("slaid", value);
				this.OnPropertyChanged("SLAId");
			}
		}
		
		/// <summary>
		/// Last SLA that was applied to this Phone Call. This field is for internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("slainvokedid")]
		public Microsoft.Xrm.Sdk.EntityReference SLAInvokedId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("slainvokedid");
			}
		}
		
		/// <summary>
		/// Shows the date and time by which the activities are sorted.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sortdate")]
		public System.Nullable<System.DateTime> SortDate
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("sortdate");
			}
			set
			{
				this.OnPropertyChanging("SortDate");
				this.SetAttributeValue("sortdate", value);
				this.OnPropertyChanged("SortDate");
			}
		}
		
		/// <summary>
		/// Shows the ID of the stage.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("stageid")]
		public System.Nullable<System.Guid> StageId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("stageid");
			}
			set
			{
				this.OnPropertyChanging("StageId");
				this.SetAttributeValue("stageid", value);
				this.OnPropertyChanged("StageId");
			}
		}
		
		/// <summary>
		/// Shows whether the phone call is open, completed, or canceled. Completed and canceled phone calls are read-only and can't be edited.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<ConfigurableEntityCloner.Test.ProxyClasses.PhoneCallState> StateCode
		{
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((ConfigurableEntityCloner.Test.ProxyClasses.PhoneCallState)(System.Enum.ToObject(typeof(ConfigurableEntityCloner.Test.ProxyClasses.PhoneCallState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.OnPropertyChanging("StateCode");
				if ((value == null))
				{
					this.SetAttributeValue("statecode", null);
				}
				else
				{
					this.SetAttributeValue("statecode", new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
				}
				this.OnPropertyChanged("StateCode");
			}
		}
		
		/// <summary>
		/// Select the phone call's status.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public System.Nullable<ConfigurableEntityCloner.Test.ProxyClasses.phonecall_statuscode> StatusCode
		{
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statuscode");
				if ((optionSet != null))
				{
					return ((ConfigurableEntityCloner.Test.ProxyClasses.phonecall_statuscode)(System.Enum.ToObject(typeof(ConfigurableEntityCloner.Test.ProxyClasses.phonecall_statuscode), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.OnPropertyChanging("StatusCode");
				if ((value == null))
				{
					this.SetAttributeValue("statuscode", null);
				}
				else
				{
					this.SetAttributeValue("statuscode", new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
				}
				this.OnPropertyChanged("StatusCode");
			}
		}
		
		/// <summary>
		/// Type a subcategory to identify the phone call type and relate the activity to a specific product, sales region, business group, or other function.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("subcategory")]
		public string Subcategory
		{
			get
			{
				return this.GetAttributeValue<string>("subcategory");
			}
			set
			{
				this.OnPropertyChanging("Subcategory");
				this.SetAttributeValue("subcategory", value);
				this.OnPropertyChanged("Subcategory");
			}
		}
		
		/// <summary>
		/// Type a short description about the objective or primary topic of the phone call.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("subject")]
		public string Subject
		{
			get
			{
				return this.GetAttributeValue<string>("subject");
			}
			set
			{
				this.OnPropertyChanging("Subject");
				this.SetAttributeValue("subject", value);
				this.OnPropertyChanged("Subject");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("subscriptionid")]
		public System.Nullable<System.Guid> SubscriptionId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("subscriptionid");
			}
			set
			{
				this.OnPropertyChanging("SubscriptionId");
				this.SetAttributeValue("subscriptionid", value);
				this.OnPropertyChanged("SubscriptionId");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timezoneruleversionnumber")]
		public System.Nullable<int> TimeZoneRuleVersionNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("timezoneruleversionnumber");
			}
			set
			{
				this.OnPropertyChanging("TimeZoneRuleVersionNumber");
				this.SetAttributeValue("timezoneruleversionnumber", value);
				this.OnPropertyChanged("TimeZoneRuleVersionNumber");
			}
		}
		
		/// <summary>
		/// Enter the account, contact, lead, or user recipients of the phone call.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("to")]
		public System.Collections.Generic.IEnumerable<Microsoft.Xrm.Sdk.Entity> To
		{
			get
			{
				Microsoft.Xrm.Sdk.EntityCollection collection = this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityCollection>("to");
				if (((collection != null) 
							&& (collection.Entities != null)))
				{
					return System.Linq.Enumerable.Cast<Microsoft.Xrm.Sdk.Entity>(collection.Entities);
				}
				else
				{
					return null;
				}
			}
			set
			{
				this.OnPropertyChanging("To");
				if ((value == null))
				{
					this.SetAttributeValue("to", value);
				}
				else
				{
					this.SetAttributeValue("to", new Microsoft.Xrm.Sdk.EntityCollection(new System.Collections.Generic.List<Microsoft.Xrm.Sdk.Entity>(value)));
				}
				this.OnPropertyChanged("To");
			}
		}
		
		/// <summary>
		/// Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("transactioncurrencyid")]
		public Microsoft.Xrm.Sdk.EntityReference TransactionCurrencyId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("transactioncurrencyid");
			}
			set
			{
				this.OnPropertyChanging("TransactionCurrencyId");
				this.SetAttributeValue("transactioncurrencyid", value);
				this.OnPropertyChanged("TransactionCurrencyId");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("traversedpath")]
		public string TraversedPath
		{
			get
			{
				return this.GetAttributeValue<string>("traversedpath");
			}
			set
			{
				this.OnPropertyChanging("TraversedPath");
				this.SetAttributeValue("traversedpath", value);
				this.OnPropertyChanged("TraversedPath");
			}
		}
		
		/// <summary>
		/// Time zone code that was in use when the record was created.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("utcconversiontimezonecode")]
		public System.Nullable<int> UTCConversionTimeZoneCode
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("utcconversiontimezonecode");
			}
			set
			{
				this.OnPropertyChanging("UTCConversionTimeZoneCode");
				this.SetAttributeValue("utcconversiontimezonecode", value);
				this.OnPropertyChanged("UTCConversionTimeZoneCode");
			}
		}
		
		/// <summary>
		/// Version number of the phone call activity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
		public System.Nullable<long> VersionNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
			}
		}
		
		/// <summary>
		/// 1:N PhoneCall_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("PhoneCall_Annotation")]
		public System.Collections.Generic.IEnumerable<ConfigurableEntityCloner.Test.ProxyClasses.Annotation> PhoneCall_Annotation
		{
			get
			{
				return this.GetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Annotation>("PhoneCall_Annotation", null);
			}
			set
			{
				this.OnPropertyChanging("PhoneCall_Annotation");
				this.SetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Annotation>("PhoneCall_Annotation", null, value);
				this.OnPropertyChanged("PhoneCall_Annotation");
			}
		}
		
		/// <summary>
		/// 1:N phonecall_connections1
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("phonecall_connections1")]
		public System.Collections.Generic.IEnumerable<ConfigurableEntityCloner.Test.ProxyClasses.Connection> phonecall_connections1
		{
			get
			{
				return this.GetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("phonecall_connections1", null);
			}
			set
			{
				this.OnPropertyChanging("phonecall_connections1");
				this.SetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("phonecall_connections1", null, value);
				this.OnPropertyChanged("phonecall_connections1");
			}
		}
		
		/// <summary>
		/// 1:N phonecall_connections2
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("phonecall_connections2")]
		public System.Collections.Generic.IEnumerable<ConfigurableEntityCloner.Test.ProxyClasses.Connection> phonecall_connections2
		{
			get
			{
				return this.GetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("phonecall_connections2", null);
			}
			set
			{
				this.OnPropertyChanging("phonecall_connections2");
				this.SetRelatedEntities<ConfigurableEntityCloner.Test.ProxyClasses.Connection>("phonecall_connections2", null, value);
				this.OnPropertyChanged("phonecall_connections2");
			}
		}
		
		/// <summary>
		/// N:1 Account_Phonecalls
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("regardingobjectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Account_Phonecalls")]
		public ConfigurableEntityCloner.Test.ProxyClasses.Account Account_Phonecalls
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Account>("Account_Phonecalls", null);
			}
			set
			{
				this.OnPropertyChanging("Account_Phonecalls");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Account>("Account_Phonecalls", null, value);
				this.OnPropertyChanged("Account_Phonecalls");
			}
		}
		
		/// <summary>
		/// N:1 Contact_Phonecalls
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("regardingobjectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Contact_Phonecalls")]
		public ConfigurableEntityCloner.Test.ProxyClasses.Contact Contact_Phonecalls
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Contact>("Contact_Phonecalls", null);
			}
			set
			{
				this.OnPropertyChanging("Contact_Phonecalls");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Contact>("Contact_Phonecalls", null, value);
				this.OnPropertyChanged("Contact_Phonecalls");
			}
		}
	}
}
