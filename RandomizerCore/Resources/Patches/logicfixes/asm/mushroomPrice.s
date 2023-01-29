.equ bluePrice, spotPrice+4
.equ redPrice, bluePrice+4
.equ briochePrice, redPrice+4
.equ croissantPrice, briochePrice+4
.equ piePrice, croissantPrice+4
.equ cakePrice, piePrice+4
.thumb
@check if breads
cmp	r4,#0x68
beq	brioche
cmp	r4,#0x69
beq	croissant
cmp	r4,#0x6A
beq	pie
cmp	r4,#0x6B
beq	cake

@check if potions
cmp	r4,#0x24
beq	red
cmp	r4,#0x25
beq	blue

@otherwise, mushroom spot
other:
ldr	r2,spotPrice
b	end

blue:
ldr	r2,bluePrice
b	end

red:
ldr	r2,redPrice
b	end

brioche:
ldr	r2,briochePrice
b	end

croissant:
ldr	r2,croissantPrice
b	end

pie:
ldr	r2,piePrice
b	end

cake:
ldr	r2,cakePrice
b	end

end:
ldr	r3,=#0x807DE6D
bx	r3

.align
.ltorg
spotPrice:
@WORD spotPrice
@WORD bluePrice
@WORD redPrice
