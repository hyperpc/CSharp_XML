<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:template match="/">
    <html>
        <head>
            <link rel="stylesheet" type="text/css" href="cd.css"/>
        </head>
        <body>
            <h2>My CD Collection</h2>
            <table border="1">
                <tr class="trcolor">
                    <th>Title</th>
                    <th>Artist</th>
                    <th>Year</th>
                    <th>Price</th>
                </tr>
                <xsl:for-each select="catalog/cd">
                <tr>
                    <td class="thleft"><xsl:value-of select="title"/></td>
                    <td style="text-align:right;"><xsl:value-of select="artist"/></td>
                    <td style="text-align:left;"><xsl:value-of select="year"/></td>
                    <td style="text-align:right;">$<xsl:value-of select="price"/></td>
                </tr>
                </xsl:for-each>
            </table>
        </body>
    </html>
    </xsl:template>
</xsl:stylesheet>