.equ bluePrice, spotPrice+4
.equ redPrice, bluePrice+4
.thumb
cmp	r4,#0x24
beq	red
cmp	r4,#0x25
beq	blue

other:
ldr	r2,spotPrice
b	end

blue:
ldr	r2,bluePrice
b	end

red:
ldr	r2,redPrice

end:
ldr	r3,=#0x807DE6D
bx	r3

.align
.ltorg
spotPrice:
@WORD spotPrice
@WORD bluePrice
@WORD redPrice
