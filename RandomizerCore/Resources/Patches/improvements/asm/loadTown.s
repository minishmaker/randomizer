.thumb
@check Link's X position
ldr	r3,=#0x300118E
ldrh	r0,[r3]
ldrh	r1,=#0x05DF
cmp	r0,r1
bhi	end

@set soldier flag
ldr	r3,=#0x2002CD5
ldrb	r0,[r3]
mov	r1,#0x08
orr	r0,r1
strb	r0,[r3]

end:
ldr	r3,=#0x804DBBD
bx	r3
