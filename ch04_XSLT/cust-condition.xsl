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
                        <th>Special Word</th>
                    </tr>
                    <xsl:for-each select="customers/customer">
                        <tr>
                            <td><xsl:value-of select="@id"/></td>
                            <td><xsl:value-of select="firstname"/></td>
                            <td><xsl:value-of select="lastname"/></td>
                            <td><xsl:value-of select="homephone"/></td>
                            <td><xsl:value-of select="notes"/></td>
                            <td>
                            <xsl:choose>
                                <xsl:when test="notes[contains(.,'new')]">new (member)</xsl:when>
                                <xsl:when test="notes[contains(.,'NICE')]">NICE (credits)</xsl:when>
                                <xsl:when test="notes[contains(.,'VIP')]">VIP (guest)</xsl:when>
                                <xsl:otherwise>Unknown</xsl:otherwise>
                            </xsl:choose>
                            </td>
                        </tr>
                    </xsl:for-each>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>