namespace ConfigurableEntityCloner.Test.ProxyClasses
{

	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("annotation")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.82")]
	public partial class Annotation : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Annotation() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "annotation";
		
		public const string EntityLogicalCollectionName = "annotations";
		
		public const string EntitySetName = "annotations";
		
		public const int EntityTypeCode = 5;
		
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
		/// Unique identifier of the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("annotationid")]
		public System.Nullable<System.Guid> AnnotationId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("annotationid");
			}
			set
			{
				this.OnPropertyChanging("AnnotationId");
				this.SetAttributeValue("annotationid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("AnnotationId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("annotationid")]
		public override System.Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.AnnotationId = value;
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the note.
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
		/// Date and time when the note was created.
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
		/// Unique identifier of the delegate user who created the annotation.
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
		/// Contents of the note's attachment.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("documentbody")]
		public string DocumentBody
		{
			get
			{
				return this.GetAttributeValue<string>("documentbody");
			}
			set
			{
				this.OnPropertyChanging("DocumentBody");
				this.SetAttributeValue("documentbody", value);
				this.OnPropertyChanged("DocumentBody");
			}
		}
		
		/// <summary>
		/// File name of the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("filename")]
		public string FileName
		{
			get
			{
				return this.GetAttributeValue<string>("filename");
			}
			set
			{
				this.OnPropertyChanging("FileName");
				this.SetAttributeValue("filename", value);
				this.OnPropertyChanged("FileName");
			}
		}
		
		/// <summary>
		/// File size of the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("filesize")]
		public System.Nullable<int> FileSize
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("filesize");
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
		/// Specifies whether the note is an attachment.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isdocument")]
		public System.Nullable<bool> IsDocument
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isdocument");
			}
			set
			{
				this.OnPropertyChanging("IsDocument");
				this.SetAttributeValue("isdocument", value);
				this.OnPropertyChanged("IsDocument");
			}
		}
		
		/// <summary>
		/// Language identifier for the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("langid")]
		public string LangId
		{
			get
			{
				return this.GetAttributeValue<string>("langid");
			}
			set
			{
				this.OnPropertyChanging("LangId");
				this.SetAttributeValue("langid", value);
				this.OnPropertyChanged("LangId");
			}
		}
		
		/// <summary>
		/// MIME type of the note's attachment.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mimetype")]
		public string MimeType
		{
			get
			{
				return this.GetAttributeValue<string>("mimetype");
			}
			set
			{
				this.OnPropertyChanging("MimeType");
				this.SetAttributeValue("mimetype", value);
				this.OnPropertyChanged("MimeType");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the note.
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
		/// Date and time when the note was last modified.
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
		/// Unique identifier of the delegate user who last modified the annotation.
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
		/// Text of the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("notetext")]
		public string NoteText
		{
			get
			{
				return this.GetAttributeValue<string>("notetext");
			}
			set
			{
				this.OnPropertyChanging("NoteText");
				this.SetAttributeValue("notetext", value);
				this.OnPropertyChanged("NoteText");
			}
		}
		
		/// <summary>
		/// Unique identifier of the object with which the note is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		public Microsoft.Xrm.Sdk.EntityReference ObjectId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("objectid");
			}
			set
			{
				this.OnPropertyChanging("ObjectId");
				this.SetAttributeValue("objectid", value);
				this.OnPropertyChanged("ObjectId");
			}
		}
		
		/// <summary>
		/// Type of entity with which the note is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objecttypecode")]
		public string ObjectTypeCode
		{
			get
			{
				return this.GetAttributeValue<string>("objecttypecode");
			}
			set
			{
				this.OnPropertyChanging("ObjectTypeCode");
				this.SetAttributeValue("objecttypecode", value);
				this.OnPropertyChanged("ObjectTypeCode");
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
		/// Unique identifier of the user or team who owns the note.
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
		/// Unique identifier of the business unit that owns the note.
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
		/// Unique identifier of the team who owns the note.
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
		/// Unique identifier of the user who owns the note.
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
		/// Prefix of the file pointer in blob storage.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("prefix")]
		public string Prefix
		{
			get
			{
				return this.GetAttributeValue<string>("prefix");
			}
		}
		
		/// <summary>
		/// workflow step id associated with the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("stepid")]
		public string StepId
		{
			get
			{
				return this.GetAttributeValue<string>("stepid");
			}
			set
			{
				this.OnPropertyChanging("StepId");
				this.SetAttributeValue("stepid", value);
				this.OnPropertyChanged("StepId");
			}
		}
		
		/// <summary>
		/// Subject associated with the note.
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
		/// Version number of the note.
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
		/// N:1 Account_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Account_Annotation")]
		public ConfigurableEntityCloner.Test.ProxyClasses.Account Account_Annotation
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Account>("Account_Annotation", null);
			}
			set
			{
				this.OnPropertyChanging("Account_Annotation");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Account>("Account_Annotation", null, value);
				this.OnPropertyChanged("Account_Annotation");
			}
		}
		
		/// <summary>
		/// N:1 Contact_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Contact_Annotation")]
		public ConfigurableEntityCloner.Test.ProxyClasses.Contact Contact_Annotation
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Contact>("Contact_Annotation", null);
			}
			set
			{
				this.OnPropertyChanging("Contact_Annotation");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.Contact>("Contact_Annotation", null, value);
				this.OnPropertyChanged("Contact_Annotation");
			}
		}
		
		/// <summary>
		/// N:1 PhoneCall_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("PhoneCall_Annotation")]
		public ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall PhoneCall_Annotation
		{
			get
			{
				return this.GetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall>("PhoneCall_Annotation", null);
			}
			set
			{
				this.OnPropertyChanging("PhoneCall_Annotation");
				this.SetRelatedEntity<ConfigurableEntityCloner.Test.ProxyClasses.PhoneCall>("PhoneCall_Annotation", null, value);
				this.OnPropertyChanged("PhoneCall_Annotation");
			}
		}
	}
}
