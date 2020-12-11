# 深入理解C#中的XML

![Cover](IMG/CSharp_XML.jpg)

> 说明：本源码并未使用书中Windows应用的实现方式，而是基于.NET Core 3.1的控制台应用.
> Happy coding!!!  

## 第1章 XML简介

属性：元数据
元素：数据

|实体引用|字符|
|:----:|:----:|
|\&lt;|<|
|\&gt;|>|
|\&amp;|&|
|\&apos;|'|
|\&quot;|"|

> 如果大段内容需要替换成实体引用，可以考虑使用\<![CDATA[...]]>包裹。XML解析器不会解析其中内容，但是内容不能包含]]>。  

DTD：Document Type Definition, ***xml.dtd***  
XML Schema, 代替DTD进行XML文档验证的技术标准  

两种操作XML的模型：流模型和文档对象模型。  
流模型又分“推”和“拉”两种模型。
“推”模型就是SAX(Simple API of XML),靠事件驱动。推荐在处理大文件时使用。  
“拉”模型是.NET的实现方案。从读取器拉去感兴趣的文档部分，不需要引发事件。  
DOM(Document Object Model)支持XPath查询。  
XSLT(Extensible Stylesheet Language Transformations)可扩展样式表转换语言，将XML文档转换为其他文本文档的语言，建立在XML和XPath之上的国际标准。  

## 第2章 使用DOM操作XML

XmlDocument, XmlProcessingInstruction, XmlElement, XmlAttribute, XmlText, XmlNode  
Load()/ LoadXml(), GetElementByTagName(), GetElementById(), SelectNodes(), SelectSingleNode()  
Save(Console.Out), CreateDocumentFragment/CreateElement/CreateAttribute/SetAttributeNode/CreateCDataSection/CreateComment(), AppendChild(), InnerXml/ InnerText,  
RemoveAll()/ ReplaceChild(), InsertAfter/ FirstChild/ LastChild / PreserveWhitespace/ NamespaceURI/ Prefix/ LocalName  

```xml
    <cust:customers xmlns:cus="http://www.customerservice.com">  
    </cust:customers>
```

NodeChanging/ NodeChanged/ NodeInserting/ NodeInserted/ NodeRemoving/ NodeRemoved  
Action/ OldParent/ NewParent/ OldValue/ NewValue/ Node  

## 第3章 应用XPath操作XML文档

> System.Xml.XPath;

常用路径表达式
|表达式|注释|
|:----|:----|
|nodename|选取该节点下的所有子节点|
|/|选取根节点|
|//|选取文档中所有符合条件的节点|
|.|选取当前节点|
|..|选取当前节点的父节点|
|@|选取属性|
|/book[1]|选取找到的第一个book元素|
|/book[last()]|选取找到的最后一个book元素|
|/book[last()-1]|选取找到的倒数第二个book元素|
|/book[position() < 3]|选取找到的前两个book元素|
|//title/text()='XML'|选取所有title的文本是XML的元素|
|//title[@lang]|选取所有包含lang属性的title元素|
|//title[@lang='eng']|选取所有包含lang属性并且等于eng的title元素|
|/book[price<35]/title|选取price小于35的book元素的title|
|//title\|//book/price|选取所有title元素和所有book节点的price元素|
|*|通配符，任意的节点元素|
|@*|通配符，任意的节点属性|
|node()|匹配任意种类的节点|

|XPath轴名称|结果|
|:----|:----|
|self|选取当前节点|
|ancestor|选取当前节点的所有先辈节点，如ancestor::book|
|ancestor-or-self|选取当前节点的所有先辈节点，包括当前节点|
|attribute|选取当前节点的所有属性，如attribute:: *|
|parent|选取当前节点的所有父节点|
|child|选取当前节点的所有子元素，如child::text(), child:: */child::price选取所有孙节点price|
|descendant|选取当前节点的所有后代元素|
|descendant-or-self|选取当前节点的所有后代元素，包括当前节点|
|preceding|选取当前节点的开始标签之前的所有节点|
|preceding-sibling|选取当前节点的开始标签之前的所有同级节点|
|following|选取当前节点结束标签之后的所有节点|
|namespace|选取当前节点的所有命名空间节点|

|运算符|描述|
|:----|:----|
|\||计算两个节点集合|
|+|加法|
|-|减法|
|*|乘法|
|div|除法|
|=|等于|
|!=|不等于|
|<|小于|
|<=|小于等于|
|>|大于|
|>=|大于等于|
|or|或|
|and|与|
|mod|取余|

XPath和XSLT、XQuery等共享函数库，包括存取函数，错误和跟踪函数，数值函数，字符串函数，时间函数，上下文函数，节点函数，布尔函数，和一般性函数，另外还有测试序列容量的函数，与QNames相关的函数，合计函数和生成序列的函数。  
参考文档：[W3Schools], [MDN]  

[W3Schools]: https://www.w3schools.com/xml/xsl_functions.asp  
[MDN]: https://developer.mozilla.org/en-US/docs/Web/XPath

XPathNavigator/ IXPathNavigator,  
XmlDocument.CreateNavigator()(可编辑Xml),  
XPathDocument.CreateNavigator()(只读Xml)  
XPathNavigator.Select()/SelectSingleNode()/SelectChildren()/SelectAncestors()/SelectDescendants()  
/MoveToAttribute()/MoveToFirstAttribute()/MoveToNextAttribute()/MoveToParent()/MoveToContent()  
/Compile()/InnerXml/OuterXml  
GetAttribute("id","")/ ReadSubtree()/ XmlWriter.Create()  
SetValue()/ DeleteSelf()  

## 第4章 用XSLT转换XML文档

XSLT: Extensible Stylesheet Language Transformation  
XSL => XSLT和XSL-FO  

xsl:stylesheet/ xsl:transform,  
xsl:template, xsl:apply-templates, value-of/copy-of,  
disable-output-escaping="yes", &lt;br&gt;&lt;/br&gt;,  
xsl:for-each/ xsl:if/ xsl:sort/ xsl:choose/ xsl:when/ xsl:otherwise  
xslt whitespace: \&#160;  
xsl:param  
XslCompiledTransform, XsltArgumentList, XsltSettings  

> CSS:  
> Cascading Style Sheet  
> Xml inner css only effect in FireFox.  

## 第5章 验证XML文档

DTD: Document Type Definitions  
XDR: XML Data Reduced
XSD: XML Schema Definition  

xsd:schema, xsd:element, xsd:complexType (mixed), xsd:attribute  
xsd:sequence, xsd:all, xsd:choice  
> DataType:
> boolean, datetime, decimal, string, int,  
> nonNegativeInteger, nonPositiveInteger, short  

> Restriction:  
> enumeration, fractionDigit, length, minExclusive, maxExclusive,  
> minLength, maxLength, minInclusive, maxInclusive, pattern, totalDigits, whiteSpace  

xsd:simpleType, xsd:restriction, xsd:enumeration, xsd:list, xsd:union,  
xsd:any, xsd:complexContent, xsd:anyType  

## 第6章 读/写XML文档

XmlReader: XmlTextReader, XmlNodeReader, XmlValidatingReader  
XmlWriter: XmlTextWriter  

XmlReader.Create(), XmlReaderSettings  
XmlWriter.Create(), XmlWriterSettings  
WriteStartDocument(), WriteStartElement(), WriteEndElement(),  
WriteAttributeString(), WriteElementString(), WriteString(), WriteCData(),  
Flush()  

## 第7章 XML和ADO.NET

SqlCommand.ExecuteXmlReader(), "Select xxx From Table For XML Raw/Auto/Path/Explicit, Elements"  
DataAdapter.Fill(DataSet, "Table"),  
DataSet.WriteXml(filefullpath, XmlWriteMode.IgnoreSchema),  
DataSet.WriteXmlSchema(filefullpath),  
XmlWriteMode.IgnoreSchema/WriteSchema/DiffGram  

DataSet.ReadXml(filefullpath)/ReadXmlSchema()/InferXmlSchema()  
DataSet < = > XmlDataDocument < = > XmlDocument  
'XmlDataDocument' is obsolete,  
XslCompiledTransform, GetElementFromRow()  

## 第8章 WCF与XML

WCF: Windows Communication Foundation  
SOA: Service Oriented  Architecture  
ServiceClass, Host, EndPoints(ABC: Address, Binding, Contract)  

> Two ways to create and using WCF:
> 1: Visual Studio
> 2: Visual Studio + SvcUtil.exe
> Note: .Net Core not support wcf anymore, you can using wcf via .Net Framework instead.
> And the sample in book is using sql server as backend db, my sample using xml file instead.

WSDL: Web Service Description Language  

## 第9章 LINQ与XML

LINQ: Language Integrated Query  
XLINQ: LINQ to XML  

var, Lambda=>, let  
XDocument, XElement, XAttribute, XNamespace,
XElement.Parse(string), Array.ForEach(), XElement.AddAnnotation(string)  
XmlSchemaSet, XElement.NamespaceName/Name/Name.ToString(), LoadOptions/SaveOptions  

## 第10章 XML序列化

序列化: 将驻留在内存中的对象转换为容易传输的格式的过程  
反序列化: 根据六重新构造对象  
三种形式：二进制序列化，XML序列化， SOAP序列化  
深序列化：序列化类的所有成员，甚至嵌套类及其所有成员，如二进制序列化  
浅序列化：进序列化类的公共成员，如XML序列化  

XmlSerializer, XmlSerializer.Serialize()/Deserialize(),  
UnknownAttribute/UnknownNode/UnknownElement,  
> System.Xml.Serialization:
> XmlRootAttribute(), XmlAttributeAttribute(), XmlElementAttribute(), XmlIgnoreAttribute(),  
> XmlArrayAttribute(), XmlArrayItemAttribute(), XmlEnumAttribute()  

SOAP: Simple Object Access Protocol  
> .Net Framework: System.Runtime.Serialization.Formatters.Soap:
> [Serializable], SoapFormatter(), formatter.Serialize()/Deserialize()  
> [OnSerializingAttribute], [OnSerializedAttribute], [OnDeserializeAttribute], [OnDeserializeAttribute]  
