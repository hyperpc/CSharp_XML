<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:template match="customers">        
        <xsl:apply-templates select="customer">
            <xsl:sort select="firstname">
            </xsl:sort>
        </xsl:apply-templates>
    </xsl:template>
</xsl:stylesheet>