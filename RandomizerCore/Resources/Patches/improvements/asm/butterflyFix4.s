.thumb
neg	r0,r0
and	r0,r2
orr	r0,r1
ldrb	r3,[r4,#0x09]
cmp	r3,#0xA8
bne	store
ldrb	r3,[r4,#0x0A]
cmp	r3,#0x70
blo	end
store:
strb	r0,[r3,#0x18]
end:
ldr	r3,=#0x809F50D
bx	r3
