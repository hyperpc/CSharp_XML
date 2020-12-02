<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:template match="customers">
        <xsl:for-each select="child::customer">
            <b>
                <xsl:value-of select="firstname" disable-output-escaping="yes"></xsl:value-of>
            </b>
            <br></br>
        </xsl:for-each>
    </xsl:template>
</xsl:stylesheet>