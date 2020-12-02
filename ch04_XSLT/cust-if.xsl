<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:template match="/">
        <html>
            <body>
                <h1>Customer Listing</h1>
                <table border="1" cellspacing="0">
                    <tr>
                        <th>CustId</th>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Home Phone</th>
                        <th>Notes</th>
                    </tr>
                    <xsl:for-each select="customers/customer">
                        <xsl:if test="firstname[text()='Annie']">
                            <tr>
                                <td>
                                    <xsl:value-of select="@id"/>
                                </td>
                                <td>
                                    <xsl:value-of select="firstname"/>
                                </td>
                                <td>
                                    <xsl:value-of select="lastname"/>
                                </td>
                                <td>
                                    <xsl:value-of select="homephone"/>
                                </td>
                                <td>
                                    <xsl:value-of select="notes"/>
                                </td>
                            </tr>
                        </xsl:if>
                    </xsl:for-each>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>