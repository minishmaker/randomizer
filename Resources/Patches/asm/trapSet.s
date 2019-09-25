.thumb
cmp	r5,#0x1B
bne	end
ldr	r0,=#0x203F1FF
strb	r6,[r0]

end:
mov	r0,r5
ldr	r3,=#0x805E1CC
mov	lr,r3
.short	0xF800
mov	r4,r0
cmp	r4,#0
beq	end2

end1:
ldr	r3,=#0x80A7455
bx	r3

end2:
ldr	r3,=#0x80A746B
bx	r3
